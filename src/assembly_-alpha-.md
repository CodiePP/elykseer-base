```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/assembly.hpp"
#include "lxr/chunk.hpp"
#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

namespace lxr {

enum tAstate { readable=1, writable=2, encrypted=4 };

struct Assembly::pimpl {
  pimpl(int n)
    : _buffer(new sizebounded<char, Chunk::size>[n])
    , _n(n)
    {}

  ~pimpl() {
    if (_buffer) {
      delete[] _buffer; }
  }

  std::string said() const { return _aid.toHex(); }

  sizebounded<char, Chunk::size> *_buffer;
  int _n;
  Key256  _aid;
  int _pos {0};
  tAstate _state {readable};

};

````
