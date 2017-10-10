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

namespace SBCLab.LXR.native

open System
open System.IO

module FsUtils = 

//#if compile_for_windows
//    let eol = @"\"
//#else
//    let eol = "/"
//#endif

#if compile_for_windows
    let sep = @"\"
#else
    let sep = "/"
#endif

#if compile_for_windows
    let cleanfp (fp : string) = fp.Replace(":", ",drive")
#else
    let cleanfp (fp : string) = fp
#endif

    let osusrgrp fp =
#if compile_for_windows
        let osusr = File.GetAccessControl(fp).GetOwner(typeof<Security.Principal.NTAccount>).ToString()
        let osgrp = File.GetAccessControl(fp).GetGroup(typeof<Security.Principal.NTAccount>).ToString()
#else
        let ufi = new Mono.Unix.UnixFileInfo(fp)
        let osusr = ufi.OwnerUser.UserName
        let osgrp = ufi.OwnerGroup.GroupName
#endif
        (osusr, osgrp)


    let fstem () = "lxr_" + Environment.MachineName + "_" + Environment.UserName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
