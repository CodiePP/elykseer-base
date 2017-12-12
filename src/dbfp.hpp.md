```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "dbctrl.hpp"
#include <string>
````

namespace [lxr](namespace.list) {

/*

```fsharp

```
*/

struct DbFpBlock {
    
};

# class DbFp : public DbCtrl&lt;std::string, DbFpBlock&gt;

{

>public:

>DbFp() {}

>virtual void inStream(std::istream &) override;

>virtual void outStream(std::ostream &) override;

>protected:

>private:

>DbFp(DbFp const &) = delete;

>DbFp & operator=(DbFp const &) = delete;

};

```cpp
} // namespace
```
