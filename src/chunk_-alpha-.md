```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/chunk.hpp"
#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

namespace lxr {

struct Chunk::pimpl {
  sizebounded<char, Chunk::size> _buffer;
};

````
