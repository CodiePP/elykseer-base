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

type key = Key256.t
type said = string

type DbKeyDat = {
    key : key
    iv : byte array
    n : int
}

type DbKey() =

    inherit DbCtrl<string, DbKeyDat>()

    let rec inInner (idb : IDb<string,DbKeyDat>) (reader : XmlTextReader) =
        //System.Console.WriteLine("type = {0} name = {1} value = {2}", reader.NodeType.ToString(), reader.Name, reader.Value)
        if reader.Name = "Key" && reader.NodeType = Xml.XmlNodeType.Element then
            let said : said = reader.GetAttribute("aid")
            let iv0 : string = reader.GetAttribute("iv")
            let iv : byte array = Key.fromHex (iv0.Length/2) iv0
            let sn : string = reader.GetAttribute("n")
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                let key = Key256.fromHex reader.Value
                let n = Int32.Parse(sn)
                let r : DbKeyDat = {
                                key = key
                              ; iv = iv
                              ; n = n
                              }
                //System.Console.WriteLine("new record: aid={0} key={1}", Key256.toHex aid, Key256.toHex r.key)
                if idb.contains said = false then
                    idb.set said r
        if reader.Read() then
            inInner idb reader

    let rec inOuter this (reader : XmlTextReader) =
        if reader.NodeType = Xml.XmlNodeType.Element && reader.Name = "Key" then
            inInner this reader
        else if reader.NodeType = Xml.XmlNodeType.EndElement && reader.Name = "Key" then
            ()
        else
            if reader.Read() then
                inOuter this reader

    member this.inStream (s : TextReader) =
        use reader = new XmlTextReader(s)
        while reader.Read() do
            if reader.Name = "DbKey" then
                inOuter this.idb reader
            ()

    member this.outStream (s : TextWriter) =
        //let refl1 = Reflection.Assembly.GetCallingAssembly()
        let refl2 = Reflection.Assembly.GetExecutingAssembly()
        //let xname = refl1.GetName()
        let aname = refl2.GetName()
        s.WriteLine("<?xml version=\"1.0\"?>")
        s.WriteLine("<DbKey xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<library><name>{0}</name><version>{1}</version></library>", aname.Name, aname.Version.ToString())
        //s.WriteLine("<program><name>{0}</name><version>{1}</version></program>", xname.Name, xname.Version.ToString())
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k v ->
             let l = sprintf "  <Key aid=\"%s\" n=\"%d\" iv=\"%s\">%s</Key>" k v.n (Key.toHex v.iv.Length v.iv) (Key256.toHex v.key) in
             s.WriteLine(l))
        s.WriteLine("</DbKey>")
        s.Flush()
