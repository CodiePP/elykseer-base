```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/backupctrl.hpp"
#include "lxr/assembly.hpp"
#include "lxr/filectrl.hpp"
#include "lxr/dbfp.hpp"
#include "lxr/dbkey.hpp"
#include "lxr/md5.hpp"
#include "lxr/sha256.hpp"

#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

#include <iostream>
#include <cstdio>

namespace lxr {

struct BackupCtrl::pimpl
{
    pimpl(Options const & o)
        : _o(o)
    {
        _ass.reset(new Assembly(o.nChunks()));
    }
    ~pimpl() {}

    bool renew_assembly();

    Options _o;
    std::unique_ptr<Assembly> _ass;
    DbFp _dbfp;
    DbKey _dbkey;
    DbFp _reffp;
    uint64_t trx_in {0UL};
    uint64_t trx_out {0UL};

    private:
    pimpl();
};

````
