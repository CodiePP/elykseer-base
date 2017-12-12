```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/dbctrl.hpp"
#include "lxr/key256.hpp"
#include <string>
````

namespace [lxr](namespace.list) {

/*

```fsharp

```
*/

```c++
struct DbKeyBlock {
    DbKeyBlock() : _n(16) {};
    //DbKeyBlock(DbKeyBlock const & o) { _key=o._key; _iv=o._iv; _n=o._n; }
    Key256 _key;
    Key256 _iv;
    int _n;
};
```

# class DbKey : public DbCtrl&lt;struct DbKeyBlock&gt;

{

>public:

>DbKey() : DbCtrl&lt;struct DbKeyBlock&gt;() {}

>virtual void [inStream](dbkey_functions.cpp.md)(std::istream &) override;

>virtual void [outStream](dbkey_functions.cpp.md)(std::ostream &) const override;

>protected:

>private:

>DbKey(DbKey const &) = delete;

>DbKey & operator=(DbKey const &) = delete;

};

```cpp
} // namespace
```
