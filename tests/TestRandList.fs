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

module TestRandList

open System
open NUnit.Framework
open SBCLab.LXR


[<Test>]
let ``make a random list``() =
    let rl = RandList.make 1 3 
    Assert.AreEqual(List.length rl, 3)
    Console.WriteLine(sprintf "random list %A" rl)

[<Test>]
let ``make an empty list``() =
    let rl = RandList.make 3 3 
    Assert.AreEqual(List.length rl, 1)
    Assert.AreEqual(rl, [3])

[<Test>]
let ``make an inverse random list``() =
    let rl = RandList.make 3 1 
    Assert.AreEqual(List.length rl, 3)
    Console.WriteLine(sprintf "random list %A" rl)
