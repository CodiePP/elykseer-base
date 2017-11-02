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
open System.Security.Cryptography

module FileCtrl = 

    type FilePath = string

    let fileLastWriteTime fp =
        try
            let finfo = new FileInfo(fp) in
            finfo.LastWriteTime
        with
        | _ -> System.DateTime.FromFileTime(1L)

    let fileDate fp =
        try
            let finfo = new FileInfo(fp) in
            finfo.LastWriteTime.ToString ("yyyyMMdd HH:mm:ss")
        with
        | _ -> ""

    let fileSize fp =
        try
            let finfo = new FileInfo(fp) in
            finfo.Length
        with
        | _ -> -1L

    let fileExists fp = File.Exists fp
    let dirExists fp = Directory.Exists fp

    let isFileReadable fp = fileExists (fp : string)

    let readDir' fp =
        try
            let dirinfo = new DirectoryInfo(fp) in
            //Printf.printf "reading dir: %s\n" fp;
            [
                for f in dirinfo.EnumerateFileSystemInfos() do
                    //Printf.printf "f: %s   attr: %A \n" f.Name f.Attributes;
                    yield f.FullName
            ]
        with
        | _ -> []


    let rec fileListRecursive fp =
        //Printf.printf "@path: %s\n" fp;
        if dirExists fp then
            (* traverse directory entries *)
            List.fold (fun fs f -> (fileListRecursive f) @ fs) [] (readDir' fp)
        else
            if fileExists fp then [ fp ]
            else []
        
