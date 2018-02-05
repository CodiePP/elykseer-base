```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include <string>
#include <ctime>

````

namespace [lxr](namespace.list) {

# class OS

{

>public:

>static const std::string [hostname](os_functions.cpp.md)();

>static const std::string [username](os_functions.cpp.md)();

>static const std::string [timestamp](os_functions.cpp.md)();

>static const std::string [time2string](os_functions.cpp.md)(time_t);

>protected:

>OS() {}

>private:

>OS(OS const &) = delete;

>OS & operator=(OS const &) = delete;

};

```cpp
} // namespace
```
