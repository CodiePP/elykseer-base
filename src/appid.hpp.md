```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/key256.hpp"

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

>static std::string [appid](appid_functions.cpp.md)();

>static Key256 [salt](appid_functions.cpp.md)();

>protected:

>AppId() = default;

>private:

>AppId(AppId const &) = delete;

>AppId & operator=(AppId const &) = delete;

};

```cpp
} // namespace
```