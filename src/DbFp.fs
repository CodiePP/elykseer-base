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
             len : int;
             osusr : string;
             osgrp : string;
             osattr : string;
             checksum : Key256.t;
             blocks : DbFpBlock list
           }

type DbFp() =

    inherit DbCtrl<string, DbFpDat>()

(**
    let rec maxFpnum' (h : t) fp n =
        let fpn = sprintf "%s-%d" fp n
        if not <| h.db.ContainsKey fpn then n
        else maxFpnum' h fp (n + 1)

    let maxFpnum (h : t) fp =
        maxFpnum' h fp 1
*)

    let rec inBlock (reader : XmlTextReader) (l : DbFpBlock list) =
        //System.Console.WriteLine(" block  type = {0} name = {1} value = {2}", reader.NodeType.ToString(), reader.Name, reader.Value)
        if reader.NodeType = Xml.XmlNodeType.Whitespace || (reader.NodeType = Xml.XmlNodeType.EndElement && reader.Name = "Fblock") then
            reader.Read() |> ignore
            inBlock reader l
        else if reader.Name = "Fblock" && reader.NodeType = Xml.XmlNodeType.Element then
            let idx = reader.GetAttribute("idx")
            let apos = reader.GetAttribute("apos")
            let fpos = reader.GetAttribute("fpos")
            let blen = reader.GetAttribute("blen")
            let clen = reader.GetAttribute("clen")
            let compr = reader.GetAttribute("compressed")
            let chksum0 = reader.GetAttribute("chksum")
            if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                let chksum = Key128.fromHex chksum0
                let aid = Key256.fromHex reader.Value
                let r : DbFpBlock = { aid = aid; idx = Int32.Parse(idx);
                                fpos = Int64.Parse(fpos); apos = Int32.Parse(apos);
                                blen = Int32.Parse(blen); clen = Int32.Parse(clen);
                                compressed = (compr = "1"); checksum = chksum
                              }
                //Printf.printfn "read : %A" r
                if reader.Read() then
                   inBlock reader (r :: l)
                else 
                   l
            else
                l
        else
            l

    let rec inInner (idb : IDb<string,DbFpDat>) (reader : XmlTextReader) =
        //System.Console.WriteLine(" inner type = {0} name = {1} value = {2}", reader.NodeType.ToString(), reader.Name, reader.Value)
        if reader.Name = "Fp" && reader.NodeType = Xml.XmlNodeType.Element then
            let fp = reader.GetAttribute("fp")
            let len = reader.GetAttribute("len")
            let osusr = reader.GetAttribute("osusr")
            let osgrp = reader.GetAttribute("osgrp")
            let attrs = reader.GetAttribute("osattr")
            let chksum0 = reader.GetAttribute("chksum")
            if reader.Read() then
                let blocks = inBlock reader []
                let chksum = Key256.fromHex chksum0
                let r : DbFpDat = { len = Int32.Parse(len);
                                osusr=osusr; osgrp=osgrp; osattr=attrs;
                                checksum = chksum; blocks = blocks
                              }
                //System.Console.WriteLine("new record: aid={0} fpos={1} apos={2} len={3} fp={4}", Key256.toHex r.aid, r.fpos, r.apos, r.len, fp)
                if not <| idb.contains fp then
                    idb.set fp r
        if reader.Read() then
            inInner idb reader

    let rec inOuter this (reader : XmlTextReader) =
        if reader.NodeType = Xml.XmlNodeType.Element && reader.Name = "Fp" then
            inInner this reader
        else if reader.NodeType = Xml.XmlNodeType.EndElement && reader.Name = "Fp" then
            ()
        else
            if reader.Read() then
                inOuter this reader

    member this.inStream (s : TextReader) =
        use reader = new XmlTextReader(s)
        while reader.Read() do
            if reader.Name = "DbFp" then
                inOuter this.idb reader
            ()
            
    member this.outStream (s : TextWriter) =
        s.WriteLine("<?xml version=\"1.0\"?>")
        s.WriteLine("<DbFp xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k v ->
             let l1 = sprintf "  <Fp fp=\"%s\" len=\"%d\" osusr=\"%s\" osgrp=\"%s\" attr=\"%s\" chksum=\"%s\">" k v.len v.osusr v.osgrp v.osattr (Key256.toHex v.checksum)
             s.WriteLine(l1)
             for b in v.blocks do
                 let l2 = sprintf "    <Fblock idx=\"%d\" apos=\"%d\" fpos=\"%d\" blen=\"%d\" clen=\"%d\" compressed=\"%s\" chksum=\"%s\">%s</Fblock>" b.idx b.apos b.fpos b.blen b.clen (if b.compressed then "1" else "0") (Key128.toHex b.checksum ) (Key256.toHex b.aid)
                 s.WriteLine(l2)
             s.WriteLine("  </Fp>")
         )
        s.WriteLine("</DbFp>")
        s.Flush()

