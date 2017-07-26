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

module TestSha256

open System
open System.IO
open NUnit.Framework
open SBCLab.LXR


[<Test>]
let ``assert Sha256 returns key with length 256 bits``() =
    let k1 = Sha256.hash_string "the tremendiously extreme very long string"
    let sk1 = Key256.toHex k1
    Assert.AreEqual(Key256.bytes k1 |> Array.length, 32)
    Assert.AreEqual(String.length sk1, 64)
    // I know it from sha256sum command
    Assert.AreEqual(sk1, "492c2d98c071c727ee6657d24d82f6f6a794ccf1719a416f9da89fdf045eccf4")


[<Test>]
let ``calculate Sha256 on file``() =
    let fname = "./obj/This_is_my_test_for_Sha256.dat" in
    File.WriteAllText(fname, "./obj/This_is_my_test_for_Sha256.dat, something you would never guess, even if asked under very motivating circumstances.\nDon't try it!");
    let k1 = Sha256.hash_file fname in
    let sk1 = Key256.toHex k1 in
    // I know it from sha256sum command
    Assert.AreEqual(sk1, "75e9073ade4d3f884e0f00db60c8f633db743c311ab75e52ccb3e7cbe06bdacd")
