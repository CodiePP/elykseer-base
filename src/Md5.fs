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

module Md5 =

    open System.IO
    open System.Security.Cryptography

    exception InvalidArgument of string

    let md5_message (m : byte array) =
        use hasher = new MD5Cng() in
        hasher.ComputeHash(m)

    let hash_bytes (m : byte array) = 
        if Array.length m < 16 then raise (InvalidArgument "message too short!")
        else md5_message m |> Key.toHex 16 |> Key128.fromHex

    let hash_string m = 
        if String.length m < 16 then raise (InvalidArgument "message too short!")
        let arr = Array.create (String.length m) (byte 0) in
        String.iteri (fun i c -> arr.[i] <- byte c) m
        hash_bytes arr

    let hash_file (fp : string) =
        if FileCtrl.fileExists fp then
            use hasher = new MD5Cng() in
            use fstr = File.OpenRead fp in
            hasher.ComputeHash(fstr) |> Key.toHex 16 |> Key128.fromHex
        else raise (InvalidArgument "cannot access file")
