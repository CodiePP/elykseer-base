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

module BackupCtrl =

    exception BadAccess
    exception ReadFailed
    exception WriteFailed

    type t

    val create : Options -> t

    val setReference : t -> DbKey -> DbFp -> unit

    val finalize : t -> unit

    val backup : t -> string -> unit

    val free : t -> int

    val bytes_in : t -> int
    val bytes_out : t -> int

    val time_encrypt : t -> int
    val time_extract : t -> int
    val time_write : t -> int

    val getDbKeys : t -> DbKey
    val getDbFp : t -> DbFp
