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

module Key256 =

    type t

    val length : int
    (** length of the key in bytes *)

    val create : unit -> t
    (** create random key *)

    val bytes : t -> byte array
    (** key as bytes *)

    val toHex : t -> string
    (** hex representation of key *)

    val fromHex : string -> t
    (** make key from hex representation *)

