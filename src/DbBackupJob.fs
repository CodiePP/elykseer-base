(*
    eLyKseeR or LXR - cryptographic data archiving software
    https://github.com/CodiePP/elykseer-base
    Copyright (C) 2017 Alexander Diemand

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*)

namespace SBCLab.LXR

open System
open System.Reflection
open System.IO
open System.Xml

type DbJobDat = {
    regexincl : string list
    regexexcl : string list
    options : Options
}

type DbBackupJob() =

    inherit DbCtrl<string, DbJobDat>()

    let rec inFilters (v : DbJobDat) (reader : XmlTextReader) : DbJobDat =
        if reader.Name = "exclude" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inFilters { v with regexexcl = v.regexexcl @ [reader.Value] } reader
            else v
        elif reader.Name = "include" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inFilters { v with regexincl = v.regexincl @ [reader.Value] } reader
            else v
        elif reader.NodeType = Xml.XmlNodeType.EndElement && reader.Name = "Filters" then
            v
        elif reader.Read() then
            inFilters v reader
        else v

    let rec inInner (v : DbJobDat) (reader : XmlTextReader) : DbJobDat =
        Console.WriteLine("inner node: {0}", reader.Name)
        let mutable continue' = true
        let mutable v' = v
        if reader.NodeType = Xml.XmlNodeType.Element && reader.Name = "Options" then
            v.options.io.inStream reader
            v' <- v
        elif reader.NodeType = Xml.XmlNodeType.Element && reader.Name = "Filters" then
            v' <- inFilters v reader
        elif reader.NodeType = Xml.XmlNodeType.EndElement && reader.Name = "Job" then
            Console.WriteLine("end of Job reached.")
            continue' <- false
        if continue' && reader.Read() then
            inInner v' reader
        else v'

    let rec inOuter (idb : IDb<string,DbJobDat>) (reader : XmlTextReader) =
        if reader.NodeType = Xml.XmlNodeType.Element && reader.Name = "Job" then
            let p = reader.GetAttribute("path")
            Console.WriteLine("job with path: {0}", p)
            let v : DbJobDat = { regexincl = []; regexexcl = []; options = new Options() }
            let v' = inInner v reader
            idb.set p v'
        if reader.Read() then
            inOuter idb reader

    member this.inStream (s : TextReader) =
        use reader = new XmlTextReader(s)
        while reader.Read() do
            if reader.NodeType = Xml.XmlNodeType.Element && reader.Name = "DbBackupJob" then
                inOuter this.idb reader
            ()

    member this.outStream (s : TextWriter) =
        //let refl1 = Reflection.Assembly.GetCallingAssembly()
        let refl2 = Reflection.Assembly.GetExecutingAssembly()
        //let xname = refl1.GetName()
        let aname = refl2.GetName()
        s.WriteLine("<?xml version=\"1.0\"?>")
        s.WriteLine("<DbBackupJob xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<library><name>{0}</name><version>{1}</version></library>", aname.Name, aname.Version.ToString())
        //s.WriteLine("<program><name>{0}</name><version>{1}</version></program>", xname.Name, xname.Version.ToString())
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k (v : DbJobDat) ->
             s.WriteLine(@"  <Job path=""{0}"">", k)
             v.options.io.outStream s
             v.regexexcl |> Seq.iter(fun x ->
                 s.WriteLine("    <exclude>{0}</exclude>", x) )
             v.regexincl |> Seq.iter(fun x ->
                 s.WriteLine("    <include>{0}</include>", x) )
             s.WriteLine("  </Job>")
             )
        s.WriteLine("</DbBackupJob>")
        s.Flush()
