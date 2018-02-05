```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/dbctrl.hpp"
#include "lxr/key256.hpp"
#include "lxr/key128.hpp"
#include "lxr/md5.hpp"
#include "boost/filesystem.hpp"

#include <string>
#include <vector>
````

namespace [lxr](namespace.list) {

/*

```fsharp
type DbFpBlock = {
             idx : int;
             apos : int;
             fpos : int64;
             blen : int;
             clen : int;
             compressed : bool;
             checksum : Key128.t;
             aid : aid
            }

type DbFpDat = {
             id : Key128.t;
             len : int64;
             osusr : string;
             osgrp : string;
             osattr : string;
             checksum : Key256.t;
             blocks : DbFpBlock list
           }

```
*/

## struct DbFpBlock
```c++
{
    DbFpBlock() : _idx(-1) {};
    DbFpBlock(int,int,uint64_t,int,int,bool,Key128&&, const Key256 &);
    int _idx, _apos;
    uint64_t _fpos;
    int _blen, _clen;
    bool _compressed;
    Key128 _checksum;
    Key256 _aid;
};
```

## struct DbFpDat
```c++
{
    static DbFpDat make(std::string const &);
    static DbFpDat fromFile(boost::filesystem::path const &);
    DbFpDat() {}
    Key128 _id;
    uint64_t _len;
    std::string _osusr, _osgrp, _osattr;
    Key256 _checksum;
    std::vector<DbFpBlock> _blocks;
};
std::ostream & operator<<(std::ostream &os, lxr::DbFpDat const & dat);
```

# class DbFp : public DbCtrl&lt;struct DbFpDat&gt;

{

>public:

>DbFp() : DbCtrl&lt;struct DbFpDat&gt;() {}

>virtual void [inStream](dbfp_functions.cpp.md)(std::istream &) override;

>virtual void [outStream](dbfp_functions.cpp.md)(std::ostream &) const override;

>protected:

>private:

>DbFp(DbFp const &) = delete;

>DbFp & operator=(DbFp const &) = delete;

};

```cpp
} // namespace
```
