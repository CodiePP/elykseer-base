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

module Logging =

    // event for logging
    let logevt = new Event<string>()
    let f_logevt = logevt.Publish

    let log () s =
        //Console.WriteLine("logging ...")
        logevt.Trigger(s)

    let prt_console =
        new Handler<string>(fun sender eventargs ->
            Console.WriteLine("{0} {1}", DateTime.Now.ToString("s"(*"yyyy-MM-dd HH:mm:ss"*)), eventargs))

    let enable_console () =
        //Console.WriteLine("enabling console output")
        f_logevt.AddHandler(prt_console)

    let disable_console () =
        f_logevt.RemoveHandler(prt_console)
