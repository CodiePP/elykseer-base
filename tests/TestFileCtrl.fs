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

module TestFileCtrl

open System
open System.IO
open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``file does not exist``() =
    Assert.False(FileCtrl.fileExists("/aasdfasdf_9423jkldkl.dkj3i42"))

[<Test>]
let ``file exists``() =
    Assert.True(FileCtrl.fileExists("/bin/sh"))

[<Test>]
let ``dir exists``() =
    Assert.True(FileCtrl.dirExists("/bin"))

[<Test>]
let ``dir does not exist - nonexistant``() =
    Assert.False(FileCtrl.dirExists("/home/Jack_the_ripper_9403094"))

[<Test>]
let ``dir does not exist - a file``() =
    Assert.False(FileCtrl.dirExists("/bin/sh"))

[<Test>]
let ``file date``() =
    let fd = FileCtrl.fileDate("/bin/sh") in
    Assert.AreEqual(fd, "20160217 21:25:57") // Linux
    //Assert.AreEqual(fd, "20170206 23:22:56") // FreeBSD

[<Test>]
let ``file size``() =
    let fs = FileCtrl.fileSize("/bin/sh") in
    Assert.AreEqual(fs, 154072) // Linux
    //Assert.AreEqual(fs, 145760) // FreeBSD

[<Test>]
let ``recursive file listing``() =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let fps = FileCtrl.fileListRecursive("/usr/share") in
    let lfps = List.length fps in
    Assert.Greater(lfps, 1000)
    stopWatch.Stop()
    let ms = stopWatch.Elapsed.TotalMilliseconds in
    printfn "time needed: %f ms for %d entries\n" ms lfps
    printfn " => %f ms per entry, %f ms per 1000" (ms / (double lfps)) (ms * 1000.0 / (double lfps))
