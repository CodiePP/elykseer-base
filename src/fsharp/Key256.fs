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

    type t = { 
        key256 : byte array;
    }

    //[<Literal>]
    let length = 256

    let nbytes = length / 8

    let create () = 
        let k = Key.create nbytes in
        { key256 = k }

    let toHex k = Key.toHex nbytes k.key256

    let fromHex s = { key256 = Key.fromHex nbytes s }

    let bytes k = k.key256


(*
#load "Random.fs";;
#load "Key.fs";;
#load "Key256.fs";;

open SBCLab.LXR;;

let k1 = Key256.create ();;
assert (Array.length (Key256.bytes k1) = 32);;
let sk1 = Key256.toHex k1;;
assert (String.length sk1 = 64);;

let k2 = Key256.fromHex sk1;;
assert (Array.length (Key256.bytes k2) = 32);;
let sk2 = Key256.toHex k2;;
assert (sk1 = sk2);;
*)
