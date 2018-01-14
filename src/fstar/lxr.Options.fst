
module LXR.Options

  type deduplication = 
    | NoDedup
    | DedupComplete
    | DedupBlock

  type options =
    | Options : n:nat {n>=4 /\ n<=256}
	      -> r:nat {r=0}
	      -> compr:bool
	      -> dedup:deduplication
	      -> options
	      
  val mk : n:nat{n>=4 /\ n<=256} -> r:nat{r=0} -> compr:bool -> dedup:deduplication -> Tot options
  
  let mk n r compr dedup = 
      Options n r compr dedup
