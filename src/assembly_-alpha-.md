```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/assembly.hpp"
#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

namespace lxr {

template <int n>
struct Assembly<n>::pimpl {
  sizebounded<char, Assembly<n>::size()> _buffer;
};

````
