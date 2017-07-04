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

module Assembly =
    type t

    exception BadState
    exception BadPosition

    val aid : t -> Key256.t
    val said : t -> string
    val pos : t -> int
    val nchunks : t -> int
    val redundancy : t -> int

    val create : Options -> t
    (** creates an empty assembly with allocated buffer *)

    val restore : Options -> Key256.t -> t
    (** restores an assembly *)

    val encrypt : t -> Key256.t -> byte array
    (** finalize assembly and encrypt with given key, returns iv *)

    val decrypt : t -> Key256.t -> byte array -> unit
    (** prepare assembly and decrypt with given key *)

    val addData : t -> byte array -> int
    (** insert data into assembly, returns the number of bytes inserted *)

    val getData : t -> int -> int -> byte array
    (** access data from assembly at pos with len, returns bytes *)

    val fpChunk : t -> int -> string
    (** compute chunk filepath *)

    //val addChunk : t -> int -> unit
    (** [addChunk a n] inserts chunk data with number [n] from file *)

    val extractChunks : t -> bool
    (** extracts all chunks to file *)

    val extractChunk : t -> int -> bool
    (** [extractChunk a n] with number [n] to file *)

    val insertChunks : t -> bool
    (** inserts all chunks from file *)

    val isEncrypted : t -> bool
    (** true if the assembly is encrypted *)

    val isWritable : t -> bool
    (** true if the assembly can be written *)

    val free : t -> int
    (** calculates the available free space in the assembly's buffer *)

    val mkchunkid : t -> int -> Key256.t
    (** make chunk identifier *)

#if DEBUG
    val showChunk : t -> int -> unit

    val calc_stream_pos : t -> int -> int * int
    (** calculate row/col pair from stream position *)

    val calc_buffer_pos : t -> int * int -> int
    (** calculate buffer position for row/col pair *)
#endif
