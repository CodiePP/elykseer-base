```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/restorectrl.hpp"

namespace lxr {

struct RestoreCtrl::pimpl
{
    pimpl(Options const & o)
        : _o(o)
    {}
    ~pimpl() {}

    Options _o;
    
    private:
    pimpl();
};

````
