```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/assembly.hpp"
#include "lxr/chunk.hpp"
#include "lxr/appid.hpp"
#include "lxr/sha256.hpp"

#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

#include <string>

namespace lxr {

enum tAstate { readable=1, writable=2, encrypted=4 };

struct Assembly::pimpl {
  pimpl(int n)
    : _buffer(new sizebounded<char, Chunk::size>[n])
    , _n(n)
  {
    _aid = mk_aid();
  }

  ~pimpl() {
    if (_buffer) {
      delete[] _buffer; }
  }

  std::string said() const { return _aid.toHex(); }

  Key256 mk_aid() const {
    Key256 k;
    std::string b(AppId::appid());
    return Sha256::hash(b.append(k.toHex()));
  }

  sizebounded<char, Chunk::size> *_buffer;
  int _n;
  Key256  _aid;
  int _pos {0};
  int _state {readable};

};

````
