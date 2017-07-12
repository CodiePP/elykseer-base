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

module internal Key =

    let char_of_int i = char i
    let int_of_char c = int c

    (** creates a random key with [n] bytes *)
    let create n =
        let buf : byte array = Array.create n (byte 0) in
        let rec nrandom i = 
            if i >= n then ()
            else
                //let r = Random.int 256 in
                (*Printf.printf "@%d %d\n" i r;*)
                buf.[i] <- (byte (Random.int 256))
                nrandom (i + 1)
        in
        nrandom 0 |> ignore
        buf

    let hex2int h =
        if ((h >= '0') && (h <= '9')) then 
            (int_of_char h) - (int_of_char '0')
        else
            if (h >= 'a') && (h <= 'f') then 
                (int_of_char h) - (int_of_char 'a') + 10
            else
                (-1)

    (** convert hexadecimal numbers to [n] bytes *)
    let fromHex n (s : string) : byte array =
        if s.Length < 2 * n then [| |]
        else
            let buf = Array.create n (byte 'x') in
            let rec convert i =
                if i >= n * 2 then ()
                else begin
                    let c1 = hex2int s.[i] in
                    let c2 = hex2int s.[(i + 1)] in
                    let x = (c1 * 16) + c2 in
                    buf.[(i / 2)] <- (byte x);
                    convert (i + 2)
                end
            in
            convert 0;
            buf
        
    (** convert [n] bytes to their hexadecimal representation *)
    let toHex n (b : byte array) =
        let rec convert s i acc =
            //System.Console.WriteLine (sprintf "@ %d for %d" i n)
            if i >= n then acc
            else begin
                let c = b.[i] in
                let x = int c in
                convert s (i + 1) (acc + Printf.sprintf "%02x" x)
            end
        in
        convert b 0 ""
