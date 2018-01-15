
module LXR.BackupCtrl

  open FStar.All
  open FStar.UInt8
  open LXR.Options
  open LXR.Assembly
  open LXR.Key256
  open LXR.Key128

  type backupctrl =
    | BackupCtrl : o : options
		 -> assembly : assembly
		 -> bytes_in : nat
		 -> bytes_out : nat
		 -> backupctrl

  val bytes_in : backupctrl -> Tot nat
  let bytes_in r = BackupCtrl?.bytes_in r

  val bytes_out : backupctrl -> Tot nat
  let bytes_out r = BackupCtrl?.bytes_in r

  val create : options -> Tot backupctrl
  let create o = 
      BackupCtrl o (Assembly.create o) 0 0

  private let blocksize = 65536

  assume val backup : backupctrl -> string -> ML unit
    (* backups a file *)

