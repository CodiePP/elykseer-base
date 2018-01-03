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

module TestAes

open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``can encrypt and decrypt``() =
    let k = Key256.create ()
    let smsg = "this is a message to be en/decrypted and compared"
    let msg = Array.create 256 (byte 0)
    for i = 0 to (String.length smsg - 1) do
        msg.[i] <- byte smsg.[i]

    printf "\noriginal plaintext: \n"
    Array.iter (fun b -> printf "%02x " b) msg

    let msg2 = Aes.encrypt k msg
    printf "\nencrypted:  \n"
    Array.iter (fun b -> printf "%02x " b) msg2

    let msg3 = Aes.decrypt k msg2
    printf "\ndecrypted:  \n"
    Array.iter (fun b -> printf "%02x " b) msg3

[<Test>]
let ``huge buffer: encrypt and decrypt``() =
    let k = Key256.create ()
    //let smsg = "0123456789"
    let smsg = Key256.create() |> Key256.toHex
    let sz = 16*Chunk.width*Chunk.height
    let msg = Array.create sz (byte 0)
    let mutable idx = 0
    while idx < sz do
        for i = 0 to (String.length smsg - 1) do
            if (idx+i) < sz then
                msg.[idx+i] <- byte smsg.[i]
        idx <- idx + 10

    printfn "\noriginal plaintext: "
    //Array.iter (fun b -> printf "%02x " b) msg

    let msg2 = Aes.encrypt k msg
    Assert.LessOrEqual(msg.Length, msg2.Length)
    Assert.AreEqual(16, msg2.Length - msg.Length)
    printfn "\nencrypted: %d" msg2.Length
    //Array.iter (fun b -> printf "%02x " b) msg2

    let msg3 = Aes.decrypt k msg2
    Assert.AreEqual(msg.Length, msg3.Length)
    printfn "\ndecrypted:  %d" msg3.Length
    //Array.iter (fun b -> printf "%02x " b) msg3
