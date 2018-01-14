```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/chunk.hpp"
#include "lxr/filectrl.hpp"

#include "sizebounded/sizebounded.ipp"

#include <iostream>

namespace lxr {

struct Chunk::pimpl {
  std::shared_ptr<sizebounded<unsigned char, Chunk::size>> _buffer;
};

````
