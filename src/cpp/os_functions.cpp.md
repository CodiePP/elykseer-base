```cpp
/*
````
<fpaste ../../src/copyright.md>
```cpp
*/

#include "lxr/os.hpp"
#include <unistd.h>
#include <chrono>
#include "date/date.h"

namespace lxr {

````
declared in [OS](os.hpp.md)

```cpp

const std::string OS::hostname()
{
#if defined( __linux__ ) || defined( __APPLE__ )
    char hostname[1024];
    hostname[1023] = '\0';
    gethostname(hostname, 1023);
    return hostname;
#else
    #ifdef _WIN32
    #error open some window
    #else
    #error Do not know how to handle this!
    #endif
#endif

}

const std::string OS::username()
{
#if defined( __linux__ ) || defined( __APPLE__ )
    char username[1024];
    username[1023] = '\0';
    getlogin_r(username, 1023);
    return username;
#else
    #ifdef _WIN32
    #error open some window
    #else
    #error Do not know how to handle this!
    #endif
#endif

}

const std::string OS::timestamp()
{
#if defined( __linux__ ) || defined( __APPLE__ )
    auto now = std::chrono::system_clock::now();
    return date::format("%Y%m%dT%H%M%S", date::floor<std::chrono::seconds>(now));
#else
    #ifdef _WIN32
    #error open some window
    #else
    #error Do not know how to handle this!
    #endif
#endif
}

const std::string OS::time2string(time_t _t)
{
#if defined( __linux__ ) || defined( __APPLE__ )
    auto ts = std::chrono::system_clock::from_time_t(_t);
    return date::format("%Y%m%dT%H%M%S", date::floor<std::chrono::seconds>(ts));
#else
    #ifdef _WIN32
    #error open some window
    #else
    #error Do not know how to handle this!
    #endif
#endif
}
```

```cpp
} // namespace
```
