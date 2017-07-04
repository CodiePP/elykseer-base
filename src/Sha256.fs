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

module Sha256 =

    open System.IO
    open System.Security.Cryptography

    exception InvalidArgument of string

    let sha256_message (m : byte array) =
        use hasher = SHA256Managed.Create() in
        hasher.ComputeHash(m)

    (** internal use only *)
(*    let toHex arr =
        if Array.length arr <> 32 then raise (InvalidArgument "Sha256 message digest should be 32 bytes long!")
        else Array.fold (fun a x -> a + Printf.sprintf "%02x" x) "" arr
        *)

    let hash_bytes (m : byte array) = 
        if Array.length m < 32 then raise (InvalidArgument "message too short!")
        else sha256_message m |> Key.toHex 32 |> Key256.fromHex

    let hash_string m = 
        if String.length m < 32 then raise (InvalidArgument "message too short!")
        let arr = Array.create (String.length m) (byte 0) in
        String.iteri (fun i c -> arr.[i] <- byte c) m
        hash_bytes arr

    let hash_file (fp : string) =
        if FileCtrl.fileExists fp then
            use hasher = SHA256Managed.Create() in
            use fstr = File.OpenRead fp in
            hasher.ComputeHash(fstr) |> Key.toHex 32 |> Key256.fromHex
        else raise (InvalidArgument "cannot access file")

