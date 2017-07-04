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
type Options =
    class
    (** ctor *)
    new : unit -> Options

    (** getters *)
    member nchunks : int
    member redundancy : int
    member fpath_chunks : string
    member fpath_db : string
    member isCompressed : bool
    member isDeduplicated : int

    (** setters *)
    member setNchunks : v:int -> unit
    member setRedundancy : v:int -> unit
    member setFpathChunks : v:string -> unit
    member setFpathDb : v:string -> unit
    member setCompression : v:bool -> unit
    member setDeduplication : v:int -> unit

    end
