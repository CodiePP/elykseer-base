```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/filectrl.hpp"
#include "lxr/md5.hpp"

#include <iostream>
````

# Test suite: utFileCtrl

on class [FileCtrl](../src/filectrl.hpp.md)

``` c++
BOOST_AUTO_TEST_SUITE( utFileCtrl )
```
## Test case: get file size, date, last write time
``` c++
BOOST_AUTO_TEST_CASE( get_file_information )
{
	const std::string fp = "/bin/sh";
	BOOST_CHECK(lxr::FileCtrl::fileExists(fp));
	BOOST_CHECK(lxr::FileCtrl::isFileReadable(fp));
	BOOST_CHECK(! lxr::FileCtrl::dirExists(fp));
	BOOST_CHECK(lxr::FileCtrl::fileDate(fp) > "2000");
	BOOST_CHECK(lxr::FileCtrl::fileSize(fp) > 600000ULL);
}
```

## Test case: get recursive directory listing
``` c++
BOOST_AUTO_TEST_CASE( get_directory_listing )
{
#ifdef _WIN32
    const std::string fp = "C:\\Windows\\System32";
#else
    const std::string fp = "/usr/share";
#endif
	auto listing = lxr::FileCtrl::fileListRecursive(fp);
	std::clog << "\nfiles: " << listing.size() << std::endl;
    BOOST_CHECK( listing.size() > 20000 );
}
```

``` c++
BOOST_AUTO_TEST_SUITE_END()
```
