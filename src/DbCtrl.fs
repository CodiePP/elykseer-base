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
open System.Collections.Generic

[<AbstractClass>]
type DbCtrl<'k,'e>() =

    let db = new SortedDictionary<'k,'e>()

    interface IDb<'k,'e> with

        member this.contains k =
            db.ContainsKey(k)

        member this.get k = try Some (db.[k]) with
                            _ -> None

        member this.set k e = db.[k] <- e
                              ()

        member this.count = db.Count

        member this.appKeys f = 
            for k in db.Keys do
                f k

        member this.appValues f =
            for e in db do
                f e.Key e.Value

        member this.union other =
            other.appValues (fun k e ->
                if not <| this.idb.contains k then
                    //Printf.printfn "including %s" (k.ToString())
                    this.idb.set k e
                //else
                    //Printf.printfn "already know: %s" (k.ToString())
            )

        (** to be overridden *)
        member this.inStream reader = ()
        member this.outStream writer = ()

    (** cast to the interface *)
    member this.idb = this :> IDb<'k,'e>

    (** cast to the interface *)
    //member this.io = this :> IStreamIO
