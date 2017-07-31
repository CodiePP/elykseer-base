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

open System.IO
open System.IO.Compression
open System.Security.Cryptography

module RestoreCtrl =

    exception BadAccess
    exception ReadFailed of string
    exception NoKey

    type FilePath = string
    type RootDir = string

    type t = {
                mutable options : Options;
                dbkey : DbKey;
                dbfp : DbFp;
                mutable assembly : Assembly.t option;
                mutable inbytes : int;
                mutable outbytes : int;
                mutable tread : int;
                mutable tdecrypt : int;
                mutable textract : int;
             }

    let blocksize = 65536

#if compile_for_windows
    let eol = "\\"
#else
    let eol = "/"
#endif

    let create () = 
        Liz.verify ()
        let o = new Options()
        o.setNchunks 256
        o.setRedundancy 0
        o.setFpathDb "/tmp/"
        o.setFpathChunks "/tmp/"
        { options = o;
          dbkey = new DbKey();
          dbfp = new DbFp();
          assembly = None;
          inbytes = 0; outbytes = 0;
          tread = 0; tdecrypt = 0; textract = 0 }

    let setOptions t o = 
        t.options <- o
        ()

    let roll_assembly ac =
        ()

    let load_assembly (ctl : t) (said : string) =
        let t0 = System.DateTime.Now in
        let kdat0 = ctl.dbkey.idb.get said in
        match kdat0 with
        | None -> raise NoKey
        | Some kdat -> 
            let aid = Key256.fromHex said in
            let options = new Options()
            options.setNchunks kdat.n
            options.setRedundancy 0
            options.setFpathChunks ctl.options.fpath_chunks
            options.setFpathDb ""
            let a = Assembly.restore options aid in
            //System.Console.WriteLine("loading assembly {0} len={1}", said, Assembly.free a)
            ctl.inbytes <- ctl.inbytes + (Assembly.nchunks a) * Chunk.width * Chunk.height
            let t1 = System.DateTime.Now in
            ctl.tread <- ctl.tread + (t1 - t0).Milliseconds + 1;
            match ctl.dbkey.idb.get said with
            | None -> raise NoKey
            | Some kd ->
                Assembly.decrypt a kd.key kd.iv
                ctl.assembly <- Some a
                //System.Console.WriteLine("decrypted assembly {0} len={1}", said, Assembly.free a)
                let t2 = System.DateTime.Now in
                ctl.tdecrypt <- ctl.tdecrypt + (t2 - t1).Milliseconds + 1;
                ()

    let prep_assembly (ctl : t) (said : string) =
        match ctl.assembly with
        | None -> load_assembly ctl said
        | Some a  -> if ((Assembly.said a) <> said) then
                        load_assembly ctl said

    let getfp ctl fp = 
        ctl.dbfp.idb.get fp

    let read_bytes (ctl : t) fp (fblock : DbFpBlock) =
        match ctl.assembly with
        | None -> ()
        | Some(assembly) ->
            let t0 = System.DateTime.Now in
            let buf0 = Assembly.getData assembly fblock.apos fblock.clen
            (* uncompress *)
            let isCompressed = fblock.compressed && GZipHeader.hasId buf0
            let buf = if isCompressed then begin
                        let buf1 : byte array = Array.zeroCreate(blocksize)
                        use mstr = new MemoryStream(buf0)
                        use gzstr = new GZipStream(mstr, CompressionMode.Decompress)
                        let decompressedbytes = gzstr.Read(buf1, 0, blocksize)
                        gzstr.Close()
                        //Printf.printf "  decompressed %d -> %d bytes\n" fblock.clen decompressedbytes
                        Array.sub buf1 0 decompressedbytes
                      end
                      else
                        buf0
            use fref = File.Open(fp, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
            let atpos = fref.Seek(int64(fblock.fpos), SeekOrigin.Begin)
            //System.Console.WriteLine("seeked to position {0}", atpos)
            let nbytes = Array.length buf
            if nbytes <> fblock.blen then
                raise <| ReadFailed (Printf.sprintf "expected %d bytes, but got %d!" fblock.blen nbytes)
            if atpos = int64(fblock.fpos) then
                fref.Write(buf, 0, nbytes)
                ctl.outbytes <- ctl.outbytes + nbytes
            else
                System.Console.WriteLine("seeked to bad position: {0}", atpos)
                raise BadAccess
            let t1 = System.DateTime.Now in
            ctl.textract <- ctl.textract + (t1 - t0).Milliseconds + 1;

    let prep_dir_hierarchy fp =
        let f = new FileInfo(fp)
        let d = f.Directory
        d.Create()

    let restore ctl (rd : RootDir) (fp' : FilePath) =
        Liz.verify ()
#if compile_for_windows
        let fp = fp'.Replace(":", ",drive")
#else
        let fp = fp'
#endif
        let fpout = if rd.EndsWith(eol) then rd + fp
                    else rd + eol + fp
        if FileCtrl.fileExists fpout then raise BadAccess; // cannot overwrite
        //System.Console.WriteLine("restore {0}",fpout)

        prep_dir_hierarchy fpout

        match ctl.dbfp.idb.get fp' with
        | None -> ()
        | Some(dat) -> 
            let bl = List.sortBy (fun (x,_)->x) <| List.map (fun b -> (b.fpos, b)) dat.blocks

            List.iteri (fun (idx : int) (_, (fblock : DbFpBlock)) -> 
                //System.Console.WriteLine("{0} : {1}@{2} -> {3}", fp, fblock.blen, fblock.apos, fblock.fpos)
                prep_assembly ctl (fblock.aid |> Key256.toHex)
                read_bytes ctl fpout fblock

                ) bl

    let finalize ctl = 
        ()

    let bytes_in ctl = ctl.inbytes
    let bytes_out ctl = ctl.outbytes

    let time_decrypt ctl = ctl.tdecrypt
    let time_read ctl = ctl.tread
    let time_extract ctl = ctl.textract

    let getDbKeys ctl = ctl.dbkey
    let getDbFp ctl = ctl.dbfp

