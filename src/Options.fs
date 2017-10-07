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

[<Sealed>]
type Options () =
    class

        (** state *)
        let mutable n = 256
        let mutable r = 0
        let mutable fpch = "/tmp"
        let mutable fpdb = "/tmp"
        let mutable compression = false
        let mutable deduplication = 0

        (** getters *)
        member this.nchunks = n
        member this.redundancy = r
        member this.fpath_chunks = fpch
        member this.fpath_db = fpdb
        member this.isCompressed = compression
        member this.isDeduplicated = deduplication

        (** setters *)
        member this.setNchunks v = n <- v
        member this.setRedundancy v = r <- v
        member this.setFpathChunks v = fpch <- v
        member this.setFpathDb v = fpdb <- v
        member this.setCompression v = compression <- v
        member this.setDeduplication v = deduplication <- v

        interface IStreamIO with
             override this.inStream reader =
                 let mutable continue' = true
                 System.Console.WriteLine("current node: {0}", reader.Name)
                 if reader.Name = "Job" && reader.NodeType = Xml.XmlNodeType.EndElement then
                    continue' <- false
                 elif reader.Name = "nchunks" && reader.NodeType = Xml.XmlNodeType.Element then
                    if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                        this.setNchunks <| Int32.Parse(reader.Value)
                        System.Console.WriteLine("     chunks = {0}", this.nchunks)
                 elif reader.Name = "redundancy" && reader.NodeType = Xml.XmlNodeType.Element then
                    if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                        this.setRedundancy <| Int32.Parse(reader.Value)
                        System.Console.WriteLine("     redundancy = {0}", this.redundancy)
                 elif reader.Name = "compression" && reader.NodeType = Xml.XmlNodeType.Element then
                    if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                        this.setCompression <| (reader.Value = "True")
                        System.Console.WriteLine("     compressed = {0}", this.isCompressed)
                 elif reader.Name = "deduplication" && reader.NodeType = Xml.XmlNodeType.Element then
                    if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                        this.setDeduplication <| Int32.Parse(reader.Value)
                        System.Console.WriteLine("     deduplication = {0}", this.isDeduplicated)
                 elif reader.Name = "fpathchunks" && reader.NodeType = Xml.XmlNodeType.Element then
                    if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                        this.setFpathChunks reader.Value
                        System.Console.WriteLine("     fpchunks = {0}", this.fpath_chunks)
                 elif reader.Name = "fpathdb" && reader.NodeType = Xml.XmlNodeType.Element then
                    if reader.Read() && reader.NodeType = Xml.XmlNodeType.Text then
                        this.setFpathDb reader.Value
                        System.Console.WriteLine("     fpathdb = {0}", this.fpath_db)
                 
                 if continue' && reader.Read() then
                    this.io.inStream reader
                 ()

             override this.outStream w =
                 w.WriteLine("<Options>")
                 w.WriteLine("  <nchunks>{0}</nchunks>", n)
                 w.WriteLine("  <redundancy>{0}</redundancy>", r)
                 w.WriteLine("  <fpathchunks>{0}</fpathchunks>", fpch)
                 w.WriteLine("  <fpathdb>{0}</fpathdb>", fpdb)
                 w.WriteLine("  <compression>{0}</compression>", compression)
                 w.WriteLine("  <deduplication>{0}</deduplication>", deduplication)
                 w.WriteLine("</Options>")
                 ()
        end
        (** cast to the interface *)
        member this.io = this :> IStreamIO

    end
