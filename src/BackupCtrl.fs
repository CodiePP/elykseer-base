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

namespace SBCLab.LXR

open System
open System.Reflection
open System.IO
open System.IO.Compression
open SBCLab.LXR.native

module BackupCtrl =

    exception BadAccess of string
    exception ReadFailed 
    exception WriteFailed

    type t = {
                options : Options;
                dbkey : DbKey;
                dbfp : DbFp;
                mutable refkey : DbKey option;
                mutable reffp : DbFp option;
                mutable assembly : Assembly.t;
                mutable inbytes : int64;
                mutable outbytes : int64;
                mutable twrite : int;
                mutable tencrypt : int;
                mutable textract : int;
             }

    let blocksize = 65536

    let create o = 
        Liz.verify ()
        let a = Assembly.create o in
        { options = o;
          dbkey = new DbKey();
          dbfp = new DbFp();
          refkey = None; reffp = None;
          assembly = a;
          inbytes = 0L; outbytes = 0L;
          twrite = 0; tencrypt = 0; textract = 0 }

    let setReference ac ks fps =
        ac.refkey <- ks
        ac.reffp <- fps
        ()

    let hasReference ac (fp : string) = 
        match ac.reffp with
        | None -> false
        | Some fps ->
            fps.idb.contains fp

    let getReferenceData ac (fp : string) =
        match ac.reffp with
        | None -> None
        | Some fps -> fps.idb.get fp

    let record_k ac said k iv =
        ac.dbkey.idb.set said 
           { key=k
           ; iv=iv
           ; n=ac.options.nchunks
           }

    let record_fp ac (fp : string) (rfp : DbFpDat) =
        ac.dbfp.idb.set fp rfp

    let roll_assembly ac =
        let t0 = DateTime.Now in
        let k = Key256.create () in
        let iv = Assembly.encrypt ac.assembly k in
        let t1 = DateTime.Now in
        ac.tencrypt <- ac.tencrypt + (t1 - t0).Milliseconds + 1
        if Assembly.extractChunks ac.assembly then
            if System.Environment.GetEnvironmentVariable("LXR_DO_ACREL") <> null then
                let fpdet : string = "lxr_" + (Assembly.said ac.assembly)
                let fpout0 = ac.options.fpath_db in
                let fpout = if fpout0.EndsWith("/") then
                                fpout0
                            else
                                fpout0 + "/"
                let refl = Reflection.Assembly.GetExecutingAssembly()
                let aname = refl.GetName()
                use s = new StreamWriter(fpout + fpdet + "_acrel.xml")
                s.WriteLine("<?xml version=\"1.0\"?>")
                s.WriteLine("<ACRel xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
                s.WriteLine("<caller>{0}</caller>", aname.Name)
                s.WriteLine("<version>{0}</version>", aname.Version.ToString())
                s.WriteLine("<host>{0}</host>", Environment.MachineName)
                s.WriteLine("<user>{0}</user>", Environment.UserName)
                s.WriteLine("<date>{0}</date>", DateTime.Now.ToString("s"))
                s.WriteLine("<Assembly id=\"{0}\" n=\"{1}\">", Assembly.said ac.assembly, Assembly.nchunks ac.assembly)
                for n = 1 to Assembly.nchunks ac.assembly do
                     s.WriteLine("  <Chunk id=\"{0}\">{1}</Chunk>", n, Assembly.mkchunkid ac.assembly n |> Key256.toHex)

                s.WriteLine("</Assembly>")
                s.WriteLine("</ACRel>")
                s.Flush()
                s.Close()

            let t2 = DateTime.Now in
            record_k ac (Assembly.said ac.assembly) k iv
            ac.textract <- ac.textract + (t2 - t1).Milliseconds + 1
        else
            Console.WriteLine("somehow failed to extract chunks...")
        ac.assembly <- Assembly.create ac.options
        ()

    let rec write_block ac cnt fp bytes fpos =
        let t0 = DateTime.Now in
        let apos = Assembly.pos ac.assembly in
        let aid = Assembly.said ac.assembly in
        let nbytes = Array.length bytes
        //Printf.printf "write bytes=%d from=%s(%d) to=%s(%d)\n" nbytes fp fpos aid apos; 
        (* calculate MD5 checksum on raw data *)
        let chksum = Md5.hash_bytes bytes

        (* compress if flag is set and minimal size reached
           must fit into assembly *)
        let mutable iscompressed = ac.options.isCompressed && nbytes > 999
        let cbytes = if iscompressed then begin
                        use mstr = new MemoryStream(blocksize+128)  // allow some more space
                        use gzstr = new GZipStream(mstr, CompressionMode.Compress)
                        gzstr.Write(bytes, 0, nbytes)
                        gzstr.Close()
                        let compressedbytes = mstr.ToArray()
                        if compressedbytes.Length < nbytes then
                            //Printf.printf "  compressed %d -> %d bytes\n" nbytes (compressedbytes.Length)
                            compressedbytes
                        else
                            // not compressed
                            iscompressed <- false
                            bytes
                     end
                     else
                        bytes

        (* return a list of blocks *)
        let mutable retblock = []

        let afree = Assembly.free ac.assembly in
        if cbytes.Length > afree then begin
            (* if we reach the end of the assembly, roll it *)
            (* first half is written uncompressed *)
            let bytes1 = bytes.[0..(afree-1)]
            let obytes1 = Assembly.addData ac.assembly bytes1
            if obytes1 <> bytes1.Length then raise WriteFailed;
            (* record data *)
            ac.inbytes <- ac.inbytes + int64(obytes1);
            ac.outbytes <- ac.outbytes + int64(obytes1);
            let fblockrec : DbFpBlock = {
                  idx = cnt
                ; apos = apos
                ; fpos = fpos
                ; blen = obytes1
                ; clen = obytes1
                ; compressed = false
                ; checksum = Md5.hash_bytes bytes1
                ; aid = Key256.fromHex aid
                }
            roll_assembly ac
            (* second half is written to new assembly *)
            retblock <- fblockrec :: (write_block ac (cnt + 1) fp bytes.[afree..] (fpos + int64(obytes1)))
        end
        else begin
            (* write (compressed) data to assembly *)
            let obytes = Assembly.addData ac.assembly cbytes in
            if obytes <> cbytes.Length then raise WriteFailed;
            (* record data *)
            ac.inbytes <- ac.inbytes + int64(nbytes);
            ac.outbytes <- ac.outbytes + int64(obytes);
            let fblockrec : DbFpBlock = {
                  idx = cnt
                ; apos = apos
                ; fpos = fpos
                ; blen = nbytes
                ; clen = obytes
                ; compressed = iscompressed
                ; checksum = chksum
                ; aid = Key256.fromHex aid
                }
            retblock <- [fblockrec]
        end

        let t1 = DateTime.Now in
        ac.twrite <- ac.twrite + (t1 - t0).Milliseconds + 1;
        retblock

    let backup (ac : t) (fp : string) =
        Liz.verify ()
        if FileCtrl.isFileReadable fp then () else raise <| BadAccess fp;
        if fp.StartsWith(@"\\") then raise <| BadAccess fp; // we do not want network shares
        let fpsz : int64 = FileCtrl.fileSize fp
        //Console.WriteLine("backup {0} with len={1}\n", fp, fpsz);
        let mutable blocks : DbFpBlock list = []
        let mutable dedupLevel = 0
        (* 0 = do backup whole file
           1 = checksum matched over complete file -> do nothing
           2 = backup diff to previous one, following list of blocks *)

        let (osusr,osgrp) = FsUtils.osusrgrp fp
        let mutable attr = File.GetLastWriteTime(fp).ToString("s")

        (* calculate checksum *)
        let chksum = Sha256.hash_file fp
        if ac.options.isDeduplicated > 0 then begin
            //Console.WriteLine("deduplication {0}", ac.options.isDeduplicated)
            (* compare checksum to reference *)
            match getReferenceData ac fp with
            | None    -> ()
            | Some d0 -> //osusr <- d0.osusr
                         //osgrp <- d0.osgrp
                         //attr <- d0.osattr
                         blocks <- d0.blocks
                         if chksum = d0.checksum then 
                            dedupLevel <- 1
                         else
                            if ac.options.isDeduplicated = 2 then
                                dedupLevel <- 2
                         ()
        end

        (* blockwise reading and writing data *)
        if dedupLevel = 0 then
            (* write blocks anew *)
            use fstr = new FileStream(fp, FileMode.Open, FileAccess.Read)
            use istr = new BinaryReader(fstr)
            let rec write2 cnt (fpos : int64) (fsz : int64) tblocks =
                if fsz > 0L then begin
                    let bytes = istr.ReadBytes(blocksize)
                    let nbytes = Array.length bytes |> int64
                    let newblocks = write_block ac cnt fp bytes fpos
                    write2 (cnt + List.length newblocks) (fpos + nbytes) (fsz - nbytes) (newblocks @ tblocks)
                end
                else
                    tblocks

            blocks <- write2 1 0L fpsz []
                        |> List.rev

        else if dedupLevel = 1 then
            Console.WriteLine("File is identical, no backup of {0}", fp)

        else if dedupLevel = 2 then
            (* follow the list of previous saved blocks and only store difference *)
            (* select only blocks within actual file size *)
            let prevblocks = List.filter 
                                (fun (block : DbFpBlock) -> block.fpos + int64(block.blen) <= fpsz)
                                blocks
            blocks <- []   // reset
            let buf : byte array = Array.zeroCreate(blocksize)
            use fstr = File.Open(fp, FileMode.Open, FileAccess.Read, FileShare.None)
            let rec rewrite (block : DbFpBlock) =
                // position in stream
                let fpos = fstr.Seek(block.fpos, SeekOrigin.Begin)
                if fpos <> block.fpos then
                    // file truncated
                    raise ReadFailed
                // read bytes
                let nread = fstr.Read(buf, 0, block.blen)
                if nread <> block.blen then
                    // block shorter
                    raise ReadFailed
                // compute checksum
                let bytes = buf.[0 .. (nread-1)]
                let chksum = Md5.hash_bytes bytes
                // compare checksum
                if chksum <> block.checksum then
                    write_block ac block.idx fp bytes fpos
                else
                    [block] // return block unchanged

            blocks <- List.collect rewrite prevblocks
                      
            (* write data beyond known blocks *)
            let (maxpos,maxidx) = List.map (fun (block : DbFpBlock) -> (block.fpos + int64(block.blen),block.idx)) blocks
                                  |> List.sortBy(fun (pos1,idx1) -> (-pos1,-idx1))  |> List.head
            Console.WriteLine("maxpos = {0}@{1}", maxidx,maxpos)

            if fpsz > maxpos then
                Console.WriteLine("neeed to write another {0} bytes.", fpsz - maxpos)
                let rec write2 cnt (fpos : int64) tblocks =
                    // position in stream
                    let fpos' = fstr.Seek(fpos, SeekOrigin.Begin)
                    if fpos' <> fpos then
                        // file truncated
                        raise ReadFailed
                    // read bytes
                    let szdiff = fpsz - fpos
                    let nminsz = min blocksize <| int(szdiff)
                    let nread = fstr.Read(buf, 0, nminsz)
                    if nread <= 0 then
                        tblocks
                    else begin
                        let bytes = buf.[0 .. (nread-1)]
                        let newblocks = write_block ac cnt fp bytes fpos
                        if fpos < fpsz then
                            write2 (cnt + List.length newblocks) (fpos + int64(nread)) (newblocks @ tblocks)
                        else
                            (newblocks @ tblocks)
                    end

                blocks <- write2 (maxidx + 1) maxpos blocks

        (* record fp *)
        let fprec : DbFpDat = {
             id = Md5.hash_string fp;
             len = fpsz; checksum = chksum; 
             osusr = osusr; osgrp = osgrp; osattr = attr;
             blocks = blocks }
        record_fp ac fp fprec
        ()

    let finalize ac = 
        let fpdet = FsUtils.fstem ()
        //Console.WriteLine("finalize with head: {0}", fpdet)
        roll_assembly ac
        let fpout0 = ac.options.fpath_db in
        let fpout = if fpout0.EndsWith(FsUtils.sep) then
                       fpout0 + fpdet
                    else
                       fpout0 + FsUtils.sep + fpdet
        use ostr1 = new StreamWriter(fpout + "_dbfp.xml")
        ac.dbfp.outStream ostr1
        ostr1.Flush()
        use ostr2 = new StreamWriter(fpout + "_dbkey.xml")
        ac.dbkey.outStream ostr2
        ostr2.Flush()

    let free ac = Assembly.free ac.assembly
    let bytes_in ac = ac.inbytes
    let bytes_out ac = ac.outbytes

    let time_encrypt ac = ac.tencrypt
    let time_write ac = ac.twrite
    let time_extract ac = ac.textract

    let getDbKeys ac = ac.dbkey
    let getDbFp ac = ac.dbfp
