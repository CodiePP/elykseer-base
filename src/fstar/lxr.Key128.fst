
module LXR.Key128

  open FStar.All
  open FStar.List
  open FStar.UInt8

  type key128 =
    | Key128 : bytes : list UInt8.t
	     -> length : nat {length = 128/8}
             -> key128

  val mk : key128
  let mk = Key128 [] (128/8)

  assume val toString : key128 -> ML (s:string{FStar.String.length s = 32})

