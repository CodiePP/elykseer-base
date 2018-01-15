
module LXR.RestoreCtrl

  open FStar.All
  open FStar.UInt8
  open LXR.Options
  open LXR.Assembly
  open LXR.Key256
  open LXR.Key128

  type restorectrl =
    | RestoreCtrl : o : options
		  -> assembly : assembly
		  -> bytes_in : nat
		  -> bytes_out : nat
		  -> restorectrl

  val bytes_in : restorectrl -> Tot nat
  let bytes_in r = RestoreCtrl?.bytes_in r

  val bytes_out : restorectrl -> Tot nat
  let bytes_out r = RestoreCtrl?.bytes_in r

  val create : options -> Tot restorectrl
  let create o = 
      RestoreCtrl o (Assembly.create o) 0 0

  private let blocksize = 65536

  assume val restore : restorectrl -> string -> string -> ML unit
    (* restores a file to root path *)

