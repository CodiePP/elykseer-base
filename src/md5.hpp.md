```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/key128.hpp"
#include "boost/filesystem.hpp"
#include <functional>

````

namespace [lxr](namespace.list) {

/*

```fsharp


    module Md5 = 
    
        val hash_string : string -> Key128.t
        (** compute hash value MD5 on input string *)
    
        val hash_bytes : byte array -> Key128.t
        (** compute hash value MD5 on input bytes *)
    
        val hash_file : string -> Key128.t
        (** compute hash over file content with given file path *)
```

*/

# class Md5

// implementations exist for [OpenSSL](md5_openssl.cpp.md) 
// and [Crypto++](md5_cryptopp.cpp.md)

{

>public:

>static Key128 [hash](md5_functions.cpp.md)(std::string const &);

>static Key128 [hash](md5_functions.cpp.md)(const char[], int length);

>// static Key128 [hash](md5_functions.cpp.md)(boost::filesystem::path const &);

>protected:

>Md5() {}

>private:

>Md5(Md5 const &) = delete;

>Md5 & operator=(Md5 const &) = delete;

};

```cpp
} // namespace
```
