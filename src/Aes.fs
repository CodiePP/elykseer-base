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

module Aes =

    open OpenSSL.Crypto

    exception BadLength

    let aes_crypt b l key n d = 
        let kbytes = Key256.bytes key
        let appid = AppId.key256
        let abytes = Key256.bytes appid
        // salt is 8 bytes
        let salt = Array.create 8 (byte 0)
        for i = 0 to 7 do
            salt.[i] <- abytes.[31-i]
        // iv is 128 bits (16 bytes)
        let iv = Array.create 16 (byte 0)

        let cc = new CipherContext(Cipher.AES_256_CBC)
        let key = cc.BytesToKey(MessageDigest.SHA256, salt, kbytes, 1, ref iv)
        if d then
            cc.Encrypt(b, key, iv)
        else
            cc.Decrypt(b, key, iv)

    let encrypt (k : Key256.t) (b : byte array) = 
        let l = Array.length b in
        if l % 16 <> 0 then raise BadLength
        else aes_crypt b l k 256 true

    let decrypt (k : Key256.t) (b : byte array) = 
        let l = Array.length b in
        if l % 16 <> 0 then raise BadLength
        else aes_crypt b l k 256 false
