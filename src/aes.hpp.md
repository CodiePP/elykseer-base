```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/key128.hpp"
#include "lxr/key256.hpp"
#include "sizebounded/sizebounded.hpp"

````

namespace [lxr](namespace.list) {

/*

```fsharp

module Aes =

    exception BadLength

    val encrypt : Key256.t -> byte array -> byte array
    (** encrypts bytes with key *)

    val decrypt : Key256.t -> byte array -> byte array
    (** decrypts bytes with key *)
```

*/

# class Aes

{

>public:

>virtual ~Aes();

>virtual int [process](aes_functions.cpp.md)(int inlen, sizebounded&lt;unsigned char, 1024&gt; & inoutbuf) = 0;

>virtual int [finish](aes_functions.cpp.md)(sizebounded&lt;unsigned char, 1024&gt; & outbuf) = 0;

>protected:

>Aes();

>struct pimpl;

>std::unique_ptr&lt;pimpl&gt; _pimpl;

};

# class AesDecrypt : public Aes

{

>public:

>AesDecrypt(Key256 const & k, Key128 const & iv);

>virtual ~AesDecrypt() = default;

>virtual int [process](aes_functions.cpp.md)(int inlen, sizebounded&lt;unsigned char, 1024&gt; & inoutbuf) override;

>virtual int [finish](aes_functions.cpp.md)(sizebounded&lt;unsigned char, 1024&gt; & outbuf) override;

>protected:

>private:

>AesDecrypt(AesDecrypt const &) = delete;

>AesDecrypt & operator=(AesDecrypt const &) = delete;

};

# class AesEncrypt : public Aes

{

>public:

>AesEncrypt(Key256 const & k, Key128 const & iv);

>virtual ~AesEncrypt() = default;

>virtual int [process](aes_functions.cpp.md)(int inlen, sizebounded&lt;unsigned char, 1024&gt; & inoutbuf) override;

>virtual int [finish](aes_functions.cpp.md)(sizebounded&lt;unsigned char, 1024&gt; & outbuf) override;

>protected:

>private:

>AesEncrypt(AesEncrypt const &) = delete;

>AesEncrypt & operator=(AesEncrypt const &) = delete;

};
```cpp
} // namespace
```