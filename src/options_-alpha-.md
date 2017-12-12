```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/options.hpp"

namespace lxr {

struct Options::pimpl {
  int _nchunks;
  int _nredundancy;
  bool _iscompressed;
  int _isdeduplicated;
  boost::filesystem::path _fpathchunks;
  boost::filesystem::path _fpathmeta;
};


````
