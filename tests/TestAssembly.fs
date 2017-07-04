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

module TestAssembly

open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``assert size of newly created assembly depends on options``() =
    let o1 = new Options()
    o1.setNchunks 32
    o1.setRedundancy 0
    o1.setFpathDb "/tmp"
    o1.setFpathChunks "/tmp"
//      nchunks = 32; redundancy = 0;
//      //appid = "e6f7ba8d1d3b8094fff9b30c7d12a5ceed38da8a14be5af8a38bfed515e00812";
//      fpath_chunks = "/tmp/";
//      fpath_db = "/tmp/"
//    }

    let a1 = Assembly.create o1
    Assert.AreEqual(Assembly.pos a1, 33)
    Assert.AreEqual(Assembly.free a1, (32 * 256 * 1024 - 33))

[<Test>]
let ``assert id of newly created assemblies is different``() =
//    let o1 : Options = {
//      nchunks = 32; redundancy = 0;
//      appid = "e6f7ba8d1d3b8094fff9b30c7d12a5ceed38da8a14be5af8a38bfed515e00812";
//      fpath_chunks = "/tmp/";
//      fpath_db = "/tmp/"
//    }
    let o1 = new Options()
    o1.setNchunks 32
    o1.setRedundancy 0
    o1.setFpathDb "/tmp"
    o1.setFpathChunks "/tmp"

    let a1 = Assembly.create o1
    let a2 = Assembly.create o1
    Assert.AreNotEqual(Assembly.said a1, Assembly.said a2)

[<Test>]
let ``can encrypt and decrypt``() =
//    let o1 : Options = {
//      nchunks = 32; redundancy = 0;
//      appid = "e6f7ba8d1d3b8094fff9b30c7d12a5ceed38da8a14be5af8a38bfed515e00812";
//      fpath_chunks = "/tmp/";
//      fpath_db = "/tmp/"
//    }
    let o1 = new Options()
    o1.setNchunks 32
    o1.setRedundancy 0
    o1.setFpathDb "/tmp"
    o1.setFpathChunks "/tmp"

    let a1 = Assembly.create o1
    Assert.AreEqual(Assembly.pos a1, 33)
    let barr = [| 1uy;2uy;3uy;4uy;5uy;6uy;7uy;8uy;9uy;0uy |]
    for i = 1 to 4000 do
        Assembly.addData a1 barr |> ignore
    done
    let k1 = Key256.create ()
    let iv = Assembly.encrypt a1 k1

    Assembly.decrypt a1 k1 iv
    //Assembly.showChunk a1 1
    //Assembly.extractChunk a1 1 |> should equal true

    Assert.AreEqual(Assembly.getData a1 33 6, [|1uy;2uy;3uy;4uy;5uy;6uy|])
