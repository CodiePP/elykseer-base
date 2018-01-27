
module LXR.Assembly

  open FStar.All
  open FStar.UInt8
  open LXR.Options
  open LXR.Chunk
  open LXR.Key256
  open LXR.Key128

  type assembly =
    | Assembly : o : options
	       -> aid : key256
	       -> pos : nat
	       -> assembly

  val aid : assembly -> Tot key256
  let aid a = Assembly?.aid a
  
  val said : assembly -> ML string
  let said a = aid a |> Key256.toString

  val pos : assembly -> Tot nat
  let pos a = Assembly?.pos a

  val nchunks : assembly -> Tot nat
  let nchunks a = Options?.n (Assembly?.o a)

  val min_nchunks : a : assembly -> Lemma
    (ensures (Options?.n (Assembly?.o a) >= 4))
  let min_nchunks a = ()
  val max_nchunks : a : assembly -> Lemma
    (ensures (Options?.n (Assembly?.o a) <= 256))
  let max_nchunks a = ()
  
  val redundancy : assembly -> Tot nat
  let redundancy a = Options?.r (Assembly?.o a)

  val create : options -> Tot assembly  
  let create o = 
      Assembly o (Key256.mk) 0

  private let rec size' (i : nat) (l : nat) : nat =
    match i with
    | 0  -> l
    | _ -> size' (i - 1) (l + Chunk.size)
  val size : assembly -> Tot nat
  let size a = size' (nchunks a) 0

(*  val lower_size : a : assembly -> Lemma
    (ensures (size a >= size' 4 0))
  let lower_size a = () *)

  assume val restore : options -> key256 -> ML assembly
    (* restores an assembly *)

  assume val encrypt : assembly -> key256 -> ML key128
    (* finalize assembly and encrypt with given key, returns iv *)

  assume val decrypt : assembly -> key256 -> key128 -> ML unit
    (* prepare assembly and decrypt with given key *)

  assume val addData : assembly -> list UInt8.t -> Tot int
    (* insert data into assembly, returns the number of bytes inserted *)

  assume val getData : assembly -> int -> int -> Tot (list UInt8.t)
    (* access data from assembly at pos with len, returns bytes *)

  assume val fpChunk : assembly -> nat -> ML string
    (* compute chunk filepath *)

  assume val extractChunks : assembly -> ML bool
    (* extracts all chunks to file *)

  assume val extractChunk : assembly -> nat -> ML bool
    (* [extractChunk a n] with number [n] to file *)

  assume val insertChunks : assembly -> ML bool
    (* inserts all chunks from file *)

  assume val isEncrypted : assembly -> Tot bool
    (* true if the assembly is encrypted *)

  assume val isWritable : assembly -> Tot bool
    (* true if the assembly can be written *)

  assume val free : assembly -> Tot nat
    (* calculates the available free space in the assembly's buffer *)

  assume val mkchunkid : assembly -> nat -> Tot key256
    (* make chunk identifier *)
