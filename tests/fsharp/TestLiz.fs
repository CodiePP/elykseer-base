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

module TestLiz

open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``days to expiration``() =
    let exp = Liz.expiring
    let now = System.DateTime.Now
    System.Console.WriteLine("days to expiration: " + (Liz.daysLeft ()).ToString())
    Assert.AreEqual((exp-now).Days, Liz.daysLeft ())

[<Test>]
let ``verify should not choke``() =
    Assert.DoesNotThrow( fun () -> Liz.verify () |> ignore )

[<Test>]
let ``text 2 base64``() =
    let s = "hello world.\n"
    let bytes = System.Text.Encoding.ASCII.GetBytes(s)
    let conv = System.Convert.ToBase64String(bytes)
    System.Console.WriteLine(s + " -> " + conv)
    Assert.AreEqual("aGVsbG8gd29ybGQuCg==", conv)
    ()

[<Test>]
let ``base64 2 text``() =
    let s = "aGVsbG8gd29ybGQuCg=="
    let conv = System.Convert.FromBase64String(s)
    let text = System.Text.Encoding.ASCII.GetString(conv)
    System.Console.WriteLine(s + " -> " + text)
    Assert.AreEqual("hello world.\n", text)
    ()

[<Test>]
let ``show gpl-3.0``() =
    System.Console.WriteLine(Liz.license)
    ()

[<Test>]
let ``show copyright``() =
    System.Console.WriteLine(Liz.copyright)
    ()
