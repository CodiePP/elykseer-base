```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/chunk.hpp"

#include "sizebounded/sizebounded.ipp"

namespace lxr {

struct Chunk::pimpl {
  std::shared_ptr<sizebounded<char, Chunk::size>> _buffer;
};

````
