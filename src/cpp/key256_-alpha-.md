```cpp
/*
```
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/key256.hpp"
#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

namespace lxr {

struct Key256::pimpl {
    sizebounded<unsigned char, 256/8> _buffer;
};

```
