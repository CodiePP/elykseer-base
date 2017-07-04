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

module internal GZipHeader =

    let gzipid1 = byte(0x1f)
    let gzipid2 = byte(0x8b)

    let header = [|
        gzipid1;
        gzipid2;
        byte(8);
        byte(0);
        byte(0);byte(0);byte(0);byte(0);
        byte(0);
        byte(3) |]

    let hasId (bytes : byte array) =
        (bytes.[0] = gzipid1) && (bytes.[1] = gzipid2)

    let hasSubheader (bytes : byte array) =
        Array.mapi (fun i e -> header.[2..9].[i]=e) bytes.[2..9]
        |> Array.forall (fun e -> e)

    let hasHeader (bytes : byte array) =
        (hasId bytes) && (hasSubheader bytes)

(*    let addHeader (bytes : byte array) =
        bytes
        let b0 = bytes.[0]
        let b1 = bytes.[1]
        if b0 = gzipid1 && b1 = gzipid2 then
            bytes
        else
            Array.append header bytes *)

(*    let removeHeader (bytes : byte array) =
        bytes
        let b0 = bytes.[0]
        let b1 = bytes.[1]
        if b0 = gzipid1 && b1 = gzipid2 then
            bytes.[10..]
        else
            bytes *)
