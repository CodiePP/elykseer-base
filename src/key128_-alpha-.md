```cpp
/*
```
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/key128.hpp"
#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

namespace lxr {

struct Key128::pimpl {
    sizebounded<char, 128/8> _buffer;
};

```
