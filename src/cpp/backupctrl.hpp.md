```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/options.hpp"
#include "lxr/dbfp.hpp"
#include "lxr/dbkey.hpp"
#include "boost/filesystem.hpp"

#include <ctime>
#include <memory>

````

namespace [lxr](namespace.list) {

/*

```fsharp
module BackupCtrl =

    exception BadAccess of string
    exception ReadFailed
    exception WriteFailed

    type t

    val create : Options -> t

    val setReference : t -> DbKey option -> DbFp option -> unit

    val finalize : t -> unit

    val backup : t -> string -> unit

    val free : t -> int

    val bytes_in : t -> int64
    val bytes_out : t -> int64

    val time_encrypt : t -> int
    val time_extract : t -> int
    val time_write : t -> int

    val getDbKeys : t -> DbKey
    val getDbFp : t -> DbFp
```

*/

# class BackupCtrl

{

>public:

>explicit [BackupCtrl](backupctrl_ctor.cpp.md)(Options const &);

>[~BackupCtrl](backupctrl_ctor.cpp.md)();

>void [setReference](backupctrl_functions.cpp.md)();

>void [finalize](backupctrl_functions.cpp.md)();

>void [backup](backupctrl_functions.cpp.md)(boost::filesystem::path const &);

>int [free](backupctrl_info.cpp.md)() const;

>uint64_t [bytes_in](backupctrl_info.cpp.md)() const;

>uint64_t [bytes_out](backupctrl_info.cpp.md)() const;

>std::time_t [time_encrypt](backupctrl_info.cpp.md)() const;

>std::time_t [time_extract](backupctrl_info.cpp.md)() const;

>std::time_t [time_write](backupctrl_info.cpp.md)() const;

>protected:

>private:

>BackupCtrl();

>struct pimpl;

>std::unique_ptr&lt;pimpl&gt; _pimpl;

>BackupCtrl(BackupCtrl const &) = delete;

>BackupCtrl & operator=(BackupCtrl const &) = delete;

};

```cpp
} // namespace
```
