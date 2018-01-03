```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/sha256.hpp"

#if CRYPTOLIB == OPENSSL
#include "openssl/sha.h"
#endif
#if CRYPTOLIB == CRYPTOPP
#include "cryptopp/sha.h"
#endif

#include <stdio.h>
#include <fstream>

namespace lxr {

````
