```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/options.hpp"

#include <iostream>
#include "pugixml.hpp"

namespace lxr {

struct Options::pimpl {
  pimpl() {
    _fpathmeta = "/tmp";
    _fpathchunks =  "/tmp";
  }
  int _nchunks{16};
  int _nredundancy {0};
  bool _iscompressed {true};
  int _isdeduplicated {2};
  boost::filesystem::path _fpathchunks;
  boost::filesystem::path _fpathmeta;
};


````
