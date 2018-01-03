```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "boost/filesystem.hpp"
#include <string>
#include <vector>
#include <ctime>
````

namespace [lxr](namespace.list) {

/*

```fsharp
module FileCtrl = 

    type FilePath = string

    val fileDate : FilePath -> string
    val fileLastWriteTime : FilePath -> System.DateTime

    val fileSize : FilePath -> int64

    val fileExists : FilePath -> bool

    val dirExists : FilePath -> bool

    val isFileReadable : FilePath -> bool

    val fileListRecursive : FilePath -> FilePath list
```

*/

# class FileCtrl

{

>public:

>static std::string [fileDate](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>static std::time_t [fileLastWriteTime](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>static uint64_t [fileSize](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>static bool [fileExists](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>static bool [isFileReadable](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>static bool [dirExists](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>static std::vector&lt;boost::filesystem::path&gt; [fileListRecursive](filectrl_functions.cpp.md)(boost::filesystem::path const &);

>protected:

>FileCtrl() {}

>private:

>FileCtrl(FileCtrl const &) = delete;

>FileCtrl & operator=(FileCtrl const &) = delete;

};

```cpp
} // namespace
```