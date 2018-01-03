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

module TestLogging

open NUnit.Framework
open SBCLab.LXR

[<Test>]
let ``default is no logging to console``() =
    Logging.log () <| "days to expiration: " + (Liz.daysLeft ()).ToString()

[<Test>]
let ``logging to console``() =
    Logging.enable_console ()
    Logging.log () <| "days to expiration: " + (Liz.daysLeft ()).ToString()

[<Test>]
let ``no more logging to console``() =
    Logging.enable_console ()
    Logging.log () <| "this is visible! but not the next one"
    Logging.disable_console ()
    Logging.log () <| "days to expiration: " + (Liz.daysLeft ()).ToString()

