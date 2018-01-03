```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include <string>
#include <memory>
#include <functional>

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

>virtual unsigned char const* [bytes](key_functions.cpp.md)() const = 0;

>virtual std::string [toHex](key_functions.cpp.md)() const;

>virtual void [fromHex](key_functions.cpp.md)(std::string const &);

>virtual void [fromBytes](key_functions.cpp.md)(unsigned char const*);

>protected:

>Key() = default;

>virtual void randomize() final;

>virtual void map(std::function&lt;void(const int, const unsigned char)&gt;) const = 0;

>virtual void transform(std::function&lt;unsigned char(const int, const unsigned char)&gt;) = 0;

>private:

};

```cpp

std::ostream & operator<<(std::ostream & os, Key const & k);

} // namespace
```
