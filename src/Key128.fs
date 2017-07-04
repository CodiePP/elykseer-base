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

module Key128 =

    type t = { 
        key128 : byte array;
    }

    //[<Literal>]
    let length = 128

    let nbytes = length / 8

    let create () = 
        let k = Key.create nbytes in
        { key128 = k }

    let toHex k = Key.toHex nbytes k.key128

    let fromHex s = { key128 = Key.fromHex nbytes s }

    let bytes k = k.key128
