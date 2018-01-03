```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "sizebounded/sizebounded.hpp"

#include "boost/filesystem.hpp"
#include <memory>

````

namespace [lxr](namespace.list) {

/*

```fsharp

module Chunk = 

    type t

    exception NoAccess
    exception AlreadyExists
    exception BadNumber

    val width : int

    val height : int
    (** constant: height of a chunk in bytes *)

    val size : int
    (** constant: size of a chunk in bytes *)

    //val num : t -> int
    //val buf : t -> byte array

    val create : unit -> t
    (** create new chunk with number set *)

    //[<CompiledName("FromFile")>]
     val fromFile : string -> t
    (** restore chunk from file given file path *)

    val toFile : t -> string -> bool
    (** store chunk to file, given file path *)

    val empty : t -> unit
    (** clear content of chunk *)

    val get : t -> int -> byte
    (** get byte at index *)

    val set : t -> int -> byte -> unit
    (** set byte at index *)

#if DEBUG
    val show : t -> unit
    (** output in hex format to console *)
#endif
```

*/

# class Chunk

{

>public:

>static constexpr int width { 256 };

>static constexpr int height { 1024 };

>static constexpr int size { width * height };

>[Chunk](chunk_ctor.cpp.md)();

>explicit [Chunk](chunk_ctor.cpp.md)(std::shared_ptr&lt;sizebounded&lt;char, Chunk::size&gt;&gt;);

>void [clear](chunk_functions.cpp.md)();

>char [get](chunk_functions.cpp.md)(int) const;

>void [set](chunk_functions.cpp.md)(int, char);

>bool [toFile](chunk_functions.cpp.md)(boost::filesystem::path const &) const;

>bool [fromFile](chunk_functions.cpp.md)(boost::filesystem::path const &);

>protected:

>private:

>struct pimpl;

>std::unique_ptr&lt;pimpl&gt; _pimpl;

>Chunk(Chunk const &) = delete;

>Chunk & operator=(Chunk const &) = delete;

};

```cpp
} // namespace
```
