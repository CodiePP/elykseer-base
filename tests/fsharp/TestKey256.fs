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

module TestKey256

open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``assert key length 256 bits = 32 bytes``() =
    let k1 = Key256.create ()
    let sk1 = Key256.toHex k1
    Assert.AreEqual(Key256.length / 8, 32)
    Assert.AreEqual(Key256.bytes k1 |> Array.length, 32)
    Assert.AreEqual(String.length sk1, 64)

[<Test>]
let ``assert key is random``() =
    let k1 = Key256.create ()
    let k2 = Key256.create ()
    //Assert.NEqual(Key256.bytes k1, Key256.bytes k2)
    Assert.AreNotEqual(Key256.bytes k1, Key256.bytes k2)

[<Test>]
let ``assert keys are equal``() =
    let k1 = Key256.create ()  // random
    let k2 = Key256.toHex k1 |> Key256.fromHex
    Assert.AreEqual(k1, k2)

[<Test>]
let ``assert modified keys are unequal``() =
    let k1 = Key256.create ()  // random
    let s1 = Key256.toHex k1
    let s2 = String.map (fun c -> if c>'0' && c<='9' then char(int(c) - 1) else c) s1
    let k2 = Key256.fromHex s2
    //System.Console.WriteLine("s1 = {0}\ns2 = {1}", s1, s2)
    Assert.AreNotEqual(k1, k2)
