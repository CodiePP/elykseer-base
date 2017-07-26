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

module TestRestoreCtrl

open NUnit.Framework
open SBCLab.LXR
open System.IO

let dir = "./obj/"

[<Test>]
let ``start and test counters``() =
    let o1 = new Options()
    o1.setNchunks 16
    o1.setRedundancy 0
    o1.setFpathDb dir
    o1.setFpathChunks dir

    let r1 = RestoreCtrl.create ()
    RestoreCtrl.setOptions r1 o1
    Assert.AreEqual(0, RestoreCtrl.bytes_in r1)
    Assert.AreEqual(0, RestoreCtrl.bytes_out r1)
    Assert.AreEqual(0, RestoreCtrl.time_decrypt r1)
    Assert.AreEqual(0, RestoreCtrl.time_extract r1)
    Assert.AreEqual(0, RestoreCtrl.time_read r1)
    //Assert.AreEqual(RestoreCtrl.free b1, (16 * 256 * 1024 - 33))

[<Test>]
let ``backup and restore some file``() =
    let o1 = new Options()
    o1.setNchunks 16
    o1.setRedundancy 0
    o1.setFpathDb dir
    o1.setFpathChunks dir

    let b1 = BackupCtrl.create o1
    let fnames = ["/usr/bin/gdb"; "/usr/bin/clang"]
    let sha256 = List.map (fun fn -> Sha256.hash_file fn |> Key256.toHex) fnames
    let mutable fsizes = 0;

    for fname in fnames do
        let fsize = int(FileCtrl.fileSize fname)
        fsizes <- fsizes + fsize
        BackupCtrl.backup b1 fname
    BackupCtrl.finalize b1

    Assert.AreEqual(fsizes, BackupCtrl.bytes_in b1)
    Assert.AreEqual(fsizes, BackupCtrl.bytes_out b1)
    Assert.Greater(BackupCtrl.time_encrypt b1, 0)
    Assert.Greater(BackupCtrl.time_extract b1, 0)
    Assert.Greater(BackupCtrl.time_write b1, 0)

    use tw = new StringWriter()
    let dbfp0 = BackupCtrl.getDbFp b1
    dbfp0.outStream tw
    let dbkey0 = BackupCtrl.getDbKeys b1
    dbkey0.outStream tw
    System.Console.WriteLine("backup: \n" + tw.ToString())

    let r1 = RestoreCtrl.create ()
    RestoreCtrl.setOptions r1 o1
    let dbfp = RestoreCtrl.getDbFp r1
    dbfp.idb.union dbfp0
    let dbkey = RestoreCtrl.getDbKeys r1
    dbkey.idb.union dbkey0
    use tw2 = new StringWriter()
    dbfp.outStream tw2
    dbkey.outStream tw2
    System.Console.WriteLine("restore: \n" + tw2.ToString())

    let outpath = dir ^ "out"
    for fname in fnames do
        let fpout = outpath + fname
        if File.Exists(fpout) then
            System.Console.WriteLine("   deleting output file {0}", fpout)
            File.Delete(fpout)
        RestoreCtrl.restore r1 outpath fname

    System.Console.WriteLine("restored {0} bytes (={1}?); took read={2} ms decrypt={3} ms extract={4} ms", RestoreCtrl.bytes_in r1, RestoreCtrl.bytes_out r1, RestoreCtrl.time_read r1, RestoreCtrl.time_decrypt r1, RestoreCtrl.time_extract r1)
    //Assert.AreEqual(fsizes, RestoreCtrl.bytes_in r1)
    Assert.AreEqual(fsizes, RestoreCtrl.bytes_out r1)
    let sha256' = List.map (fun fn -> Sha256.hash_file (outpath + fn) |> Key256.toHex) fnames
    System.Console.WriteLine("compare sha256 = {0}", List.zip sha256 sha256')
    List.map (fun (a:string,b:string) -> Assert.AreEqual(a, b)) <| List.zip sha256 sha256' |> ignore

[<Test>]
let ``compressed backup and restore some file``() =
    let o1 = new Options()
    o1.setNchunks 16
    o1.setRedundancy 0
    o1.setFpathDb dir
    o1.setFpathChunks dir
    o1.setCompression true

    let b1 = BackupCtrl.create o1
    let fname = dir + "testfile.dat"
    let fsize = 120873;

    (** prepare file *)
    use ostr = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.Write)
    use owriter = new BinaryWriter(ostr)
    owriter.Seek(0, SeekOrigin.Begin) |> ignore
    let rec bwrite n =
        if n > 0 then
            owriter.Write('9')
            bwrite (n - 1)
    bwrite fsize
    owriter.Flush()
    owriter.Close()
    ostr.Close()

    (** backup *)
    BackupCtrl.backup b1 fname
    BackupCtrl.finalize b1

    Assert.AreEqual(fsize, BackupCtrl.bytes_in b1)
    Assert.GreaterOrEqual(fsize, BackupCtrl.bytes_out b1)
    Assert.Greater(BackupCtrl.time_encrypt b1, 0)
    Assert.Greater(BackupCtrl.time_extract b1, 0)
    Assert.Greater(BackupCtrl.time_write b1, 0)

    use tw = new StringWriter()
    let dbfp0 = BackupCtrl.getDbFp b1
    dbfp0.outStream tw
    let dbkey0 = BackupCtrl.getDbKeys b1
    dbkey0.outStream tw
    System.Console.WriteLine("backup: \n" + tw.ToString())

    let r1 = RestoreCtrl.create ()
    RestoreCtrl.setOptions r1 o1
    let dbfp = RestoreCtrl.getDbFp r1
    dbfp.idb.union dbfp0
    let dbkey = RestoreCtrl.getDbKeys r1
    dbkey.idb.union dbkey0
    use tw2 = new StringWriter()
    dbfp.outStream tw2
    dbkey.outStream tw2
    System.Console.WriteLine("restore: \n" + tw2.ToString())

    let outpath = dir + "out2"
    let fpout = outpath + fname
    if File.Exists(fpout) then
        System.Console.WriteLine("   deleting output file {0}", fpout)
        File.Delete(fpout)
    RestoreCtrl.restore r1 outpath fname

    System.Console.WriteLine("restored {0} bytes (={1}?); took read={2} ms decrypt={3} ms extract={4} ms", RestoreCtrl.bytes_in r1, RestoreCtrl.bytes_out r1, RestoreCtrl.time_read r1, RestoreCtrl.time_decrypt r1, RestoreCtrl.time_extract r1)
    Assert.AreEqual(fsize, RestoreCtrl.bytes_out r1)

