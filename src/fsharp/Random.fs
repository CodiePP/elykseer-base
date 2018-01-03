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
open sharpPRNG

module Random = 

    let mutable private rg = null

#if USE_SYSTEM_RANDOM
    do rg <- new System.Random(); Console.WriteLine("random initialized.")

    let int max = rg.Next(max)
#else
    do rg <- new sharpPRNG.MT19937()
(*
       let refl = System.Reflection.Assembly.GetAssembly(typeof<sharpPRNG.MT19937>)
       let ms = refl.GetModules()
       Console.WriteLine("mt19937 initialized from:")
       Array.iter (fun m -> Console.WriteLine("   {0}", m.ToString())) ms
*)

    let int max =
        let t = rg.get_real((float32)max) in
        (int)t
#endif
