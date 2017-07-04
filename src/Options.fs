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

    end
