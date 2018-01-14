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
module RestoreCtrl =

    exception BadAccess
    exception ReadFailed of string
    exception NoKey

    type t

    val create : unit -> t
    val setOptions : t -> Options -> unit

    type FilePath = string
    type RootDir = string

    val restore : t -> RootDir -> FilePath -> unit

    val bytes_in : t -> int64
    val bytes_out : t -> int64

    val time_decrypt : t -> int
    val time_extract : t -> int
    val time_read : t -> int

    val getDbKeys : t -> DbKey
    val getDbFp : t -> DbFp
```

*/

# class RestoreCtrl

{

>public:

>explicit [RestoreCtrl](restorectrl_ctor.cpp.md)(Options const &);

>[~RestoreCtrl](restorectrl_ctor.cpp.md)();

>void [restore](restorectrl_functions.cpp.md)(boost::filesystem::path const & root, std::string const & fp);

>uint64_t [bytes_in](restorectrl_info.cpp.md)() const;

>uint64_t [bytes_out](restorectrl_info.cpp.md)() const;

>std::time_t [time_decrypt](restorectrl_info.cpp.md)() const;

>std::time_t [time_extract](restorectrl_info.cpp.md)() const;

>std::time_t [time_read](restorectrl_info.cpp.md)() const;

>protected:

>private:

>RestoreCtrl();

>struct pimpl;

>std::unique_ptr&lt;pimpl&gt; _pimpl;

>RestoreCtrl(RestoreCtrl const &) = delete;

>RestoreCtrl & operator=(RestoreCtrl const &) = delete;

};

```cpp
} // namespace
```
