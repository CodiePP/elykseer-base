```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/key256.hpp"
#include "sizebounded/sizebounded.hpp"

#include <memory>

````

namespace [lxr](namespace.list) {

/*

```fsharp

module Assembly =
    type t

    exception BadState
    exception BadPosition

    val aid : t -> Key256.t
    val said : t -> string
    val pos : t -> int
    val nchunks : t -> int
    val redundancy : t -> int

    val create : Options -> t
    (** creates an empty assembly with allocated buffer *)

    val restore : Options -> Key256.t -> t
    (** restores an assembly *)

    val encrypt : t -> Key256.t -> byte array
    (** finalize assembly and encrypt with given key, returns iv *)

    val decrypt : t -> Key256.t -> byte array -> unit
    (** prepare assembly and decrypt with given key *)

    val addData : t -> byte array -> int
    (** insert data into assembly, returns the number of bytes inserted *)

    val getData : t -> int -> int -> byte array
    (** access data from assembly at pos with len, returns bytes *)

    val fpChunk : t -> int -> string
    (** compute chunk filepath *)

    //val addChunk : t -> int -> unit
    (** [addChunk a n] inserts chunk data with number [n] from file *)

    val extractChunks : t -> bool
    (** extracts all chunks to file *)

    val extractChunk : t -> int -> bool
    (** [extractChunk a n] with number [n] to file *)

    val insertChunks : t -> bool
    (** inserts all chunks from file *)

    val isEncrypted : t -> bool
    (** true if the assembly is encrypted *)

    val isWritable : t -> bool
    (** true if the assembly can be written *)

    val free : t -> int
    (** calculates the available free space in the assembly's buffer *)

    val mkchunkid : t -> int -> Key256.t
    (** make chunk identifier *)

```

*/

# class Assembly

{

>public:

>explicit [Assembly](assembly_ctor.cpp.md)(int n);

>int size() const;

>static constexpr int datasz { 1024*4 };

>int [getData](assembly_functions.cpp.md)(int, int, sizebounded&lt;char,datasz&gt; &) const;

>int [addData](assembly_functions.cpp.md)(int, sizebounded&lt;char, datasz&gt; const &);

>Key256 [mkChunkId](assembly_functions.cpp.md)(int idx) const;

>bool [encrypt](assembly_functions.cpp.md)(Key256 const & k, Key256 & iv);

>bool [decrypt](assembly_functions.cpp.md)(Key256 const & k);

>bool [extractChunk](assembly_functions.cpp.md)(int idx) const;

>bool [extractChunks](assembly_functions.cpp.md)() const;

>bool [insertChunks](assembly_functions.cpp.md)();

>int [free](assembly_functions.cpp.md)() const;

>bool [isReadable](assembly_functions.cpp.md)() const;

>bool [isWritable](assembly_functions.cpp.md)() const;

>bool [isEncrypted](assembly_functions.cpp.md)() const;

>protected:

>private:

>struct pimpl;

>std::unique_ptr&lt;pimpl&gt; _pimpl;

>Assembly();

>Assembly(Assembly const &) = delete;

>Assembly & operator=(Assembly const &) = delete;

};

```cpp
} // namespace
```
