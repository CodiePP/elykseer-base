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

type filepath = string
type aid = Key256.t

type DbFpBlock = {
             idx : int;
             apos : int;
             fpos : int64;
             blen : int;
             clen : int;
             compressed : bool;
             checksum : Key128.t;
             aid : aid
            }

type DbFpDat = {
             id : Key128.t;
             len : int64;
             osusr : string;
             osgrp : string;
             osattr : string;
             checksum : Key256.t;
             blocks : DbFpBlock list
           }

type DbFp() =

    inherit DbCtrl<string, DbFpDat>()

    let emptyFp : DbFpDat = { id = Key128.create();
                              len=0L; osusr=""; osgrp=""; osattr="";
                              checksum = Key256.fromHex "deadbeefcafecafedeadbeefcafecafedeadbeefcafecafedeadbeefcafecafe"; blocks=[] }

    let rec inBlock (reader : XmlTextReader) (l : DbFpBlock list) =
        //System.Console.WriteLine(" block  type = {0} name = {1} value = {2}", reader.NodeType.ToString(), reader.Name, reader.Value)
        //if reader.NodeType = Xml.XmlNodeType.Whitespace || (reader.NodeType = Xml.XmlNodeType.EndElement && reader.Name = "Fblock") then
        if reader.Name = "Fblock" && reader.NodeType = Xml.XmlNodeType.EndElement then
            //Printf.printfn "end of Fblock: %d blocks" (List.length l)
            l
        elif reader.Name = "Fblock" && reader.NodeType = Xml.XmlNodeType.Element then
            let idx = reader.GetAttribute("idx")
            let apos = reader.GetAttribute("apos")
            let fpos = reader.GetAttribute("fpos")
            let blen = reader.GetAttribute("blen")
            let clen = reader.GetAttribute("clen")
            let compr = reader.GetAttribute("compressed")
            let chksum = Key128.fromHex <| reader.GetAttribute("chksum")
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                let aid = Key256.fromHex reader.Value
                let r : DbFpBlock = { aid = aid; idx = Int32.Parse(idx);
                                fpos = Int64.Parse(fpos); apos = Int32.Parse(apos);
                                blen = Int32.Parse(blen); clen = Int32.Parse(clen);
                                compressed = (compr = "1"); checksum = chksum
                              }
                //Printf.printfn "read : %A" r
                reader.Read() |> ignore
                inBlock reader (r :: l)
            else
                //Printf.printfn "no text in Fblock: %d blocks" (List.length l)
                l
        else
            reader.Read() |> ignore
            inBlock reader l

    let rec inAttrs (record : DbFpDat) (reader : XmlTextReader) =
        reader.Read() |> ignore
        if reader.Name = "Fattrs" && reader.NodeType = Xml.XmlNodeType.EndElement then
            record
        elif reader.Name <> "Fattrs" && reader.NodeType = Xml.XmlNodeType.EndElement then
            inAttrs record reader // skip over
        elif reader.Name = "osusr" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inAttrs {record with osusr = reader.Value} reader
            else record
        elif reader.Name = "osgrp" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inAttrs {record with osgrp = reader.Value} reader
            else record
        elif reader.Name = "length" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inAttrs {record with len = Int64.Parse(reader.Value)} reader
            else record
        elif reader.Name = "last" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inAttrs {record with osattr = reader.Value} reader
            else record
        elif reader.Name = "chksum" && reader.NodeType = Xml.XmlNodeType.Element then
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                inAttrs {record with checksum = Key256.fromHex reader.Value} reader
            else record
        else
            record

    let rec inInner (idb : IDb<string,DbFpDat>) (fp : string, (record : DbFpDat)) (reader : XmlTextReader) =
        //System.Console.WriteLine(" inner type = {0} name = {1} value = {2}", reader.NodeType.ToString(), reader.Name, reader.Value)
        if reader.Name = "Fp" && reader.NodeType = Xml.XmlNodeType.EndElement then
            //System.Console.WriteLine("new DbFpDat @ {0} with {1} blocks", fp, List.length record.blocks)
            if not <| idb.contains fp then
                idb.set fp record
            
        elif reader.Name = "Fp" && reader.NodeType = Xml.XmlNodeType.Element then
            let fp' = reader.GetAttribute("fp")
            let id = Key128.fromHex <| reader.GetAttribute("id")
            if reader.Read() then
                inInner idb (fp', { record with id = id }) reader

        elif reader.Name = "Fblock" && reader.NodeType = Xml.XmlNodeType.Element then
            let blocks = inBlock reader record.blocks
            inInner idb (fp, { record with blocks = blocks }) reader

        elif reader.Name = "Fattrs" && reader.NodeType = Xml.XmlNodeType.Element then
            let record' = inAttrs record reader
            inInner idb (fp, record') reader

        elif reader.Read() then
            inInner idb (fp, record) reader

    let rec inOuter this (reader : XmlTextReader) =
        if reader.Name = "DbFp" && reader.NodeType = Xml.XmlNodeType.EndElement then
            ()
        elif reader.Name = "Fp" && reader.NodeType = Xml.XmlNodeType.Element then
            inInner this ("",emptyFp) reader
        if reader.Read() then
            inOuter this reader
        else ()

    member this.inStream (s : TextReader) =
        use reader = new XmlTextReader(s)
        while reader.Read() do
            if reader.Name = "DbFp" then
                inOuter this.idb reader
            ()
            
    member this.outStream (s : TextWriter) =
        //let refl1 = Reflection.Assembly.GetCallingAssembly()
        let refl2 = Reflection.Assembly.GetExecutingAssembly()
        //let xname = refl1.GetName()
        let aname = refl2.GetName()
        s.WriteLine("<?xml version=\"1.0\"?>")
        s.WriteLine("<DbFp xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<library><name>{0}</name><version>{1}</version></library>", aname.Name, aname.Version.ToString())
        //s.WriteLine("<program><name>{0}</name><version>{1}</version></program>", xname.Name, xname.Version.ToString())
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k v ->
             let l1 = sprintf "  <Fp fp=\"%s\" id=\"%s\">" k (Key128.toHex v.id)
             s.WriteLine(l1)
             let l2 = sprintf "     <Fattrs><osusr>%s</osusr><osgrp>%s</osgrp><length>%d</length><last>%s</last><chksum>%s</chksum></Fattrs>" v.osusr v.osgrp v.len v.osattr (Key256.toHex v.checksum)
             s.WriteLine(l2)
             for b in v.blocks do
                 let l2 = sprintf "    <Fblock idx=\"%d\" apos=\"%d\" fpos=\"%d\" blen=\"%d\" clen=\"%d\" compressed=\"%s\" chksum=\"%s\">%s</Fblock>" b.idx b.apos b.fpos b.blen b.clen (if b.compressed then "1" else "0") (Key128.toHex b.checksum ) (Key256.toHex b.aid)
                 s.WriteLine(l2)
             s.WriteLine("  </Fp>")
         )
        s.WriteLine("</DbFp>")
        s.Flush()

