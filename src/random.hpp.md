```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "mt19937.hpp"

````

namespace [lxr](namespace.list) {

/*

```fsharp


module Random =

    val int: int -> int
    (** up to max, returns uint32 *)
```

*/

# class Random

{

>public:

>[Random](random_ctor.cpp.md)();

>uint32_t [random](random_functions.cpp.md)() const;

>protected:

>private:

> std::unique_ptr&lt;prngCpp::MT19937&gt; _rng;

>Random(Random const &) = delete;

>Random & operator=(Random const &) = delete;

};

```cpp
} // namespace
```