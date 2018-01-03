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

module XmlInput =

    open System.IO
    open System.Xml
    open System.Xml.XPath

    type t = { nav : XPathNavigator
               mutable cur : XPathNodeIterator
             }

    let fromFile (fp : string) (nm : string) =
        let fstr = File.OpenRead fp
        let xd = new XPathDocument(fstr)
        let nav = xd.CreateNavigator()
        { nav = nav; cur = nav.SelectChildren(nm, "http://spec.sbclab.com/lxr/v1.0") }

    let query (q : string) xp =
        xp.cur <- xp.nav.Select(q)
        xp

    let value (n : string) xp =
        xp.nav.SelectSingleNode(n).Value

    let attribute (n : string) xp =
        xp.nav.GetAttribute(n, "")

