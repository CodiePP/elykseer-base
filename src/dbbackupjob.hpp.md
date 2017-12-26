```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/dbctrl.hpp"
#include "lxr/options.hpp"

#include <string>
#include <vector>
#include <regex>
#include <utility>
````

namespace [lxr](namespace.list) {

/*

```fsharp
type DbJobDat = {
    regexincl : Regex list
    regexexcl : Regex list
    options : Options
    paths : string list
}
```
*/

## struct DbJobDat
```c++
{
    DbJobDat();
    void include(std::string const &);
    void exclude(std::string const &);
    void addFile(std::string const &);
    void addDirectory(std::string const &);
    void addRecursive(std::string const &);

    std::vector<std::string> _strincl;
    std::vector<std::string> _strexcl;
    std::vector<std::regex> _regexincl;
    std::vector<std::regex> _regexexcl;
    Options _options {Options::defaults()};
    std::vector<std::pair<std::string,std::string>> _paths;
};
std::ostream & operator<<(std::ostream &os, lxr::DbJobDat const & dat);
```

# class DbBackupJob : public DbCtrl&lt;struct DbJobDat&gt;

{

>public:

>DbBackupJob() : DbCtrl&lt;struct DbJobDat&gt;() {}

>virtual void [inStream](dbbackupjob_functions.cpp.md)(std::istream &) override;

>virtual void [outStream](dbbackupjob_functions.cpp.md)(std::ostream &) const override;

>protected:

>private:

>DbBackupJob(DbBackupJob const &) = delete;

>DbBackupJob & operator=(DbBackupJob const &) = delete;

};

```cpp
} // namespace
```
