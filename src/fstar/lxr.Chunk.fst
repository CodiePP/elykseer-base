
module LXR.Chunk

  open FStar.All
  open FStar.UInt8

  val size : nat
  let size = 262144

  type chunk =
    | Chunk : 
          buf : list UInt8.t (*{List.length buf = size}*)
        -> chunk

  val mklist2 : (n:nat) -> (list UInt8.t) -> Tot (list UInt8.t)
  let rec mklist2 n xs = 
    match n with
    | 0 -> xs
    | _ -> mklist2 (n-1) ((UInt8.uint_to_t 0) :: xs)
  val mklist2_list_length : (n:nat) -> (l1:(list UInt8.t)) -> Lemma
    (requires (n >= 0))
    (ensures (List.length (mklist2 n l1) == (List.length l1) + n))
  let mklist2_list_length n l1 = admit()
    
  val mklist : (n:nat) -> Tot (list UInt8.t)
  let mklist n = mklist2 n []
  val mklist_list_length : (n:nat) -> Lemma
    (requires (n >= 0))
    (ensures (List.length (mklist n) == n))
  let mklist_list_length n = admit()

  val mk : chunk
  let mk = Chunk (mklist size)
