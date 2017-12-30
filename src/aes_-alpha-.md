```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/aes.hpp"
#include "sizebounded/sizebounded.ipp"

#if CRYPTOLIB == OPENSSL
#include "openssl/conf.h"
#include "openssl/evp.h"
#include "openssl/err.h"
#endif

#if CRYPTOLIB == CRYPTOPP
#include "cryptopp/filters.h"
#include "cryptopp/modes.h"
#include "cryptopp/aes.h"
#endif

#include <iostream>

namespace lxr {

````
