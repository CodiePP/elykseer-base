
module LXR.Key256

  open FStar.All
  open FStar.List
  open FStar.UInt8

  type key256 =
    | Key256 : bytes : list UInt8.t
	     -> length : nat {length = 256/8}
             -> key256

  val mk : key256
  let mk = Key256 [] (256/8)

  assume val toString : key256 -> ML (s:string{FStar.String.length s = 64})
  (*let toString k = "some representation"*)
