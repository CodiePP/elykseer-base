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

module TestRandom

open System
open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``distribution of random value between 0 and 256``() =
    let count = 100000 in
    let ndom = 256 in
    let arr = Array.create ndom (int 0) in   // number is < 256
    for i=1 to count do
        let rnd = Random.int ndom in    // generates numbers < 256
        assert (rnd < ndom)
        arr.[rnd] <- arr.[rnd] + 1

    for i=0 to (ndom - 1) do
        //Console.WriteLine (sprintf "@ %d => %d ?= %d" i arr.[i] (100000 / 256))
        Assert.Greater(arr.[i], count / ndom * 8 / 10) // > 80% mean
        Assert.Less(arr.[i], count / ndom * 12 / 10) // < 120% mean

[<Test>]
let ``distribution of random value between 0 and 1``() =
    let count = 100000 in
    let ndom = 2 in
    let arr = Array.create ndom (int 0) in    // number is 0 or 1
    for i=1 to count do
        let rnd = Random.int ndom in    // generates 0s and 1s
        arr.[rnd] <- arr.[rnd] + 1

    for i=0 to 1 do
        //Console.WriteLine (sprintf "@ %d => %d ?= %d" i arr.[i] (100000 / 2))
        Assert.Greater(arr.[i], count / ndom * 8 / 10) // > 80% mean
        Assert.Less(arr.[i], count / ndom * 12 / 10) // < 120% mean
