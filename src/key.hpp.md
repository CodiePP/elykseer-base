```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include <string>
#include <memory>

````

namespace [lxr](namespace.list) {

/*

```fsharp

module internal Key =

    val create : int -> byte array
    (** create random key with length *)

    val toHex : int -> byte array -> string
    (** hex representation of key with length *)

    val fromHex : int -> string -> byte array
    (** make key from hex representation with length *)
````

*/

# class Key

{

>public:

>virtual int length() const = 0;

>virtual char* [bytes](key_functions.cpp.md)() const;

>virtual std::string [toHex](key_functions.cpp.md)() const;

>virtual void [fromHex](key_functions.cpp.md)(std::string const &);

>virtual void [fromBytes](key_functions.cpp.md)(const char *);

>protected:

>[Key](key_ctor.cpp.md)(int bits);

>private:

>std::unique_ptr&lt;char&gt; _buffer;

};

```cpp
} // namespace
```