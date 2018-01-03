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

open Chunk
open Key256
open System
open System.IO

module Assembly =

    type Atype = Writable | Encrypted | Readable

    type t = { 
               aid : Key256.t;
               options : Options;
               mutable pos : int;
               mutable data : byte array;
               mutable atype : Atype
               mutable iv : byte array;
             }

    exception BadState
    exception BadPosition

    (** accessors *)

    let aid a = a.aid
    let said a = Key256.toHex a.aid
    let pos a = a.pos
    let nchunks a = a.options.nchunks
    let redundancy a = a.options.redundancy
    let isEncrypted a = a.atype = Encrypted
    let isWritable a = a.atype = Writable
    let isReadable a = a.atype = Writable || a.atype = Readable

    (** assertions *)
    let assertWritable a = if isWritable a then () else raise BadState
    let assertEncrypted a = if isEncrypted a then () else raise BadState
    let assertReadable a = if isReadable a then () else raise BadState

    (** constants *)

    let nrows = Chunk.height
    let wchunk = Chunk.width
    let buflen a = nrows * wchunk * nchunks a

    let free a =
        let buflen = buflen a in
        let pos = pos a in
        if pos < buflen then (buflen - pos)
        else 0

    (** calculate row/col pair from stream position *)
    let calc_stream_pos (a : t) i =
        let k = nrows in
        (i % k, i / k) (* row,col - swapped *)

    (** calculate buffer position for row/col pair *)
    let calc_buffer_pos a (row,col) =
        let k = (nchunks a) * wchunk in
        row * k + col

    (** make assembly identifier 
     *  this depends on [appid] + random number
     *)
    let mk_aid () =
        AppId.appid + (Key256.create () |> Key256.toHex)
        |> Sha256.hash_string

    (** make chunk identifier 
     *  only depends on [aid] and [cid]
     *)
    let mkchunkid a n = 
        (said a) + (Printf.sprintf "%03dch%03d" n n)
        |> Sha256.hash_string

    (** create empty assembly *)
    let create o =
        { options = o; pos = 33;
          aid = mk_aid ();
          atype = Writable;
          data = Array.create ((o.nchunks) * wchunk * nrows) (byte 'z')
          iv = Array.empty }

    (** calculate file path for a chunk *)
    let fpChunk a n = 
        let fpout0 = a.options.fpath_chunks
        let fpout = if fpout0.EndsWith("/") then
                       fpout0
                    else
                       fpout0 + "/"
        let cid = mkchunkid a n |> Key256.toHex
        fpout + cid.Substring(62, 2) + "/" + cid + ".dat"

    (** given position in chunk, return calculated position in buffer *)
    let calc_chunk_pos a n i =
        let crow = i / Chunk.width in
        let ccol = i % Chunk.width in
        let col = (n - 1) * Chunk.width + ccol in
        let pos = calc_buffer_pos a (crow, col) in
        pos

    (** chunk number start at 1, fill one byte after another *)
    let fillChunk' a n =
        let c = Chunk.create () in
        let cw = Chunk.size in
        let rec iter num limit f =
            if num >= limit then ()
            else begin
                f num; iter (num + 1) limit f 
            end in
        iter 0 cw (fun idx -> let byte = a.data.[(calc_chunk_pos a n idx)] in
                              Chunk.set c idx byte)
        ;
        c

    (** chunk number start at 1, fill n bytes in each step *)
    let fillChunk a n =
        //let width = 32 in (* <<<< change here *)
        let c = Chunk.create () in
        let cw = Chunk.size in
        let rec iter num limit f =
            if num >= limit then ()
            else begin
                f num; iter (num + 1) limit f 
            end in
        iter 0 cw (fun idx -> let byte = a.data.[(calc_chunk_pos a n idx)] in
                              Chunk.set c idx byte)
        ;
        c

    (** copies chunk with number [n] into the buffer *)
    //let addChunk a n = failwith "to be implemented"

#if DEBUG
    let showChunk a n =
        let c = fillChunk a n in
        Chunk.show c
#endif

    (** prepare directory hierarchy of chunks *)
    let prepare_dirhierarchy (fp0 : string) =
        let fp = if fp0.EndsWith("/") then
                    fp0
                 else
                    fp0 + "/"
        let l = ["0";"1";"2";"3";"4";"5";"6";"7";"8";"9";"a";"b";"c";"d";"e";"f"]
        for a in l do
            for b in l do
                let dp = (fp + a + b)
                if not <| FileCtrl.dirExists dp then
                    Directory.CreateDirectory dp |> ignore

    (** extract data for chunk with number [n] *)
    let extractChunk a n =
        assertEncrypted a;
        let c = fillChunk a n in
        let fp = fpChunk a n in
        try Chunk.toFile c fp with
        | e -> Console.WriteLine("failed to write chunk to {0}: {1}",fp,e.Message)
               false

    let extractChunks a =
        assertEncrypted a;
        prepare_dirhierarchy a.options.fpath_chunks
        let lcid = RandList.make 1 (nchunks a) in
        List.forall (extractChunk a) lcid

    (** insert data for chunk with number [n] *)
    let insertChunk' a n c =
        let cw = Chunk.size in
        let rec iter num limit f =
            if num >= limit then ()
            else begin
                f num; iter (num + 1) limit f 
            end in
        iter 0 cw (fun idx -> let byte = Chunk.get c idx in
                              a.data.[(calc_chunk_pos a n idx)] <- byte)

    let insertChunk a n =
        assertWritable a;
        let fp = fpChunk a n in
        //System.Console.WriteLine("insert chunk {0}", fp)
        try let c = Chunk.fromFile fp in
            insertChunk' a n c
            true with
        | _ -> false

    (** returns true if all chunks were read *)
    let insertChunks a =
        assertWritable a;
        let lcid = RandList.make 1 (nchunks a) in
        List.forall (insertChunk a) lcid

    (** restore assembly *)
    let  restore o aid =
        let r = {
          options = o; pos = 33;
          aid = aid;
          atype = Writable;
          data = Array.create (o.nchunks * wchunk * nrows) (byte 'w')
          iv = Array.empty }
        //Console.WriteLine("restoring assembly with {0} bytes", r.data.Length)
        if insertChunks r then
            r.atype <- Encrypted
        r

    (** add data from bytes, non-locally storing in the buffer *)
    let addData a d =
        assertWritable a;
        let apos0 = a.pos in
        let len = Array.length d in
        let rec putchar idx = 
            if (a.pos >= buflen a) || (idx >= len) then (a.pos - apos0)
            else
                let rowcol = calc_stream_pos a (apos0 + idx) in
                let bufpos = calc_buffer_pos a rowcol in
                let byte = d.[idx] in
                a.data.[bufpos] <- byte;
                a.pos <- a.pos + 1;
                putchar (idx + 1)
        in
        putchar 0

    (** get data from the buffer, returns allocated bytes *)
    let getData a p n =
        assertReadable a;
        //Console.WriteLine("get data from {0}@{1} |data|={2}", n, p, a.data.Length)
        if n <= 0 then raise BadPosition;
        if p < 0 then raise BadPosition;
        else 
            let buf = Array.create n (byte '0') in
            let rec getchar idx = 
                let rowcol = calc_stream_pos a (p + idx) in
                let bufpos = calc_buffer_pos a rowcol in
                try buf.[idx] <- a.data.[bufpos]
                with _ -> Console.WriteLine("exception at idx={0} bufpos={1} |a.data|={2}", idx, bufpos, a.data.Length)
                if idx + 1 < n then getchar (idx + 1)
                else () in
            getchar 0;
            buf

    (** dump assembly's buffer to file *)
(*    let dump2file a ext =
        let fp = "/tmp/" + (said a) + "." + ext
        use str = File.Open(fp, FileMode.CreateNew, FileAccess.Write, FileShare.None)
        str.Write(a.data, 0, a.data.Length)
        str.Flush() *)

    (** decrypt : encrypted -> writable *)
    let decrypt a k iv =
        assertEncrypted a;

        (* dump unencrypted buffer *)
        //dump2file a "before_decryption"

        (* decrypt buffer *)
        let dbuf = Array.concat [ iv ; a.data ]
        //Console.WriteLine("before decrypt: len={0}", dbuf.Length)
        a.data <- Aes.decrypt k dbuf
        //Console.WriteLine("after decrypt: len={0}", a.data.Length)
        (* change state *)
        a.atype <- Readable;

        (* transpose first 32 bytes *)
        for pos = 0 to 31 do
            let pos0 = calc_buffer_pos a (pos+1, 0)  // column 0
            let pos1 = calc_buffer_pos a (0, pos)  // row 0
            let b = a.data.[pos0]
            //Console.WriteLine(sprintf "(%i,%i) -> %2x @ %i->%i" 0 pos b pos0 pos1)
            a.data.[pos1] <- b

        (* dump unencrypted buffer *)
        //dump2file a "after_decryption"
                
        ()

    (** encrypt : writable -> encrypted *)
    let encrypt a k =
        assertWritable a;
        (* transpose first 32 bytes *)
        for pos = 0 to 31 do
            let pos0 = calc_buffer_pos a (0, pos)  // row 0
            let pos1 = calc_buffer_pos a (pos+1, 0)  // column 0
            let b = a.data.[pos0]
            //Console.WriteLine(sprintf "(%i,%i) -> %2x @ %i->%i" 0 pos b pos0 pos1)
            a.data.[pos1] <- b

        (* enter random salt in first 32 bytes *)
        let secret = Key256.create () |> Key256.bytes in
        Array.iteri (fun i b -> a.data.[i] <- b) secret

        (* dump unencrypted buffer *)
        //dump2file a "before_encryption"

        (* encrypt buffer *)
        let newdata = Aes.encrypt k a.data
        let iv = 
            if newdata.Length = a.data.Length then
                a.data <- newdata
                Array.empty
            else
                a.data <- newdata.[16..]
                newdata.[..15]

        (* dump unencrypted buffer *)
        //dump2file a "after_encryption"

        (* change state *)
        a.atype <- Encrypted;
        iv

