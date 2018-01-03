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

module Chunk = 

    type t

    exception NoAccess
    exception AlreadyExists
    exception BadNumber

    val width : int

    val height : int
    (** constant: height of a chunk in bytes *)

    val size : int
    (** constant: size of a chunk in bytes *)

    //val num : t -> int
    //val buf : t -> byte array

    val create : unit -> t
    (** create new chunk with number set *)

    //[<CompiledName("FromFile")>]
     val fromFile : string -> t
    (** restore chunk from file given file path *)

    val toFile : t -> string -> bool
    (** store chunk to file, given file path *)

    val empty : t -> unit
    (** clear content of chunk *)

    val get : t -> int -> byte
    (** get byte at index *)

    val set : t -> int -> byte -> unit
    (** set byte at index *)

#if DEBUG
    val show : t -> unit
    (** output in hex format to console *)
#endif
