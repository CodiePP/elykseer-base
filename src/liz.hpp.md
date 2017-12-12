```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include <string>

````

namespace [lxr](namespace.list) {

/*

```fsharp

module Liz = 

    type t

    exception Expired

    val expiring : System.DateTime

    val daysLeft : unit -> int

    val verify : unit -> unit

    val license : string

    val copyright : string

    val version : string
```

*/

# class Liz

{

>public:

>static const bool [verify](liz_functions.cpp.md)();

>static const int [daysLeft](liz_functions.cpp.md)();

>static const std::string [license](liz_functions.cpp.md)();

>static const std::string [copyright](liz_functions.cpp.md)();

>static const std::string [version](liz_functions.cpp.md)();

>static const std::string [name](liz_functions.cpp.md)();

>protected:

>Liz() {}

>private:

>Liz(Liz const &) = delete;

>Liz & operator=(Liz const &) = delete;

};

```cpp
} // namespace
```