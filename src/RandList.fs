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

open System

module 
#if RELEASE
    internal
#endif
    RandList =

    (* list of integers within range *)
    let range i j =
        let i' = min i j in
        let j' = max i j in
        let rec range' idx ll =
            if idx <= j' then range' (idx + 1) (idx :: ll)
            else ll
        in
        let ll = range' i' [] in
        if (i < j) then List.rev ll else ll

    (* randomly permutate list *)
    let rec permutation list =
        let rec extract acc n = function
            | [] -> raise <| Exception "not found"
            | h :: t -> if n = 0 then (h, acc @ t) 
                        else extract (h::acc) (n-1) t in
        let extract_rand list len =
            extract [] (Random.int len) list in
        let rec aux acc list len =
            if len = 0 then acc else
                let picked, rest = extract_rand list len in
                aux (picked :: acc) rest (len-1) in
        aux [] list (List.length list)

    let make n0 n1 =
        let list = range n0 n1 in
        permutation list

 
