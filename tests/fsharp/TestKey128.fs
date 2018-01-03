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

module TestKey128

open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``assert key length 128 bits = 16 bytes``() =
    let k1 = Key128.create ()
    let sk1 = Key128.toHex k1
    Assert.AreEqual(Key128.length / 8, 16)
    Assert.AreEqual(Key128.bytes k1 |> Array.length, 16)
    Assert.AreEqual(String.length sk1, 32)

[<Test>]
let ``assert key is random``() =
    let k1 = Key128.create ()
    let k2 = Key128.create ()
    Assert.AreNotEqual(Key128.bytes k1, Key128.bytes k2)
