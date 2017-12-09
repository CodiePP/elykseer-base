```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/key256.hpp"

#include <string>

````

namespace [lxr](namespace.list) {

/*

```fsharp

module internal AppId =

    val appid : string

    val salt : Key256.t
```

*/

# class AppId

{

>public:

>static const Key256 [salt](appid_functions.cpp.md)();

>static const std::string [appid](appid_functions.cpp.md)();


>protected:

>AppId() {}

>private:

>AppId(AppId const &) = delete;

>AppId & operator=(AppId const &) = delete;

};

```cpp
} // namespace
```
