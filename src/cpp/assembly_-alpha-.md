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
#include "lxr/aes.hpp"
#include "lxr/options.hpp"
#include "lxr/filectrl.hpp"
#include "lxr/fsutils.hpp"

#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

#include "boost/optional.hpp"

#include <string>
#include <iostream>

namespace lxr {

enum tAstate { readable=1, writable=2, encrypted=4 };

struct Assembly::pimpl {
  pimpl(Key256 const & aid, int n)
    : _chunks(new Chunk[n])
    , _n(n)
    , _aid(aid)
  { }

  pimpl(int n)
    : pimpl(mk_aid(), n)
  { }

  ~pimpl() {
    if (_chunks) {
      delete[] _chunks; }
  }

  std::string said() const { return _aid.toHex(); }

  Key256 mk_aid() const {
    Key256 k;
    std::string b(AppId::appid());
    return Sha256::hash(b.append(k.toHex()));
  }

  Chunk *_chunks;
  int _n;
  Key256  _aid;
  int _pos {0};
  int _state {writable};

private: 
pimpl() {}
};

````
readable