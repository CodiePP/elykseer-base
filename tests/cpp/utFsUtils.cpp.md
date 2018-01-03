```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/fsutils.hpp"
#include "lxr/md5.hpp"

#include <iostream>
````

# Test suite: utFsUtils

on class [FsUtils](../src/fsutils.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utFsUtils )
```
## Test case: get file stem
```cpp
BOOST_AUTO_TEST_CASE( file_stem )
{
	std::string fstem = lxr::FsUtils::fstem();
	//std::clog << lxr::FsUtils::fstem() << std::endl;
	//std::string md5 = "30616da777172b41a2417ddb986f2ca0";
	BOOST_CHECK_EQUAL(fstem.substr(0,4), "lxr_");
}
```

## Test case: get file stem
```cpp
BOOST_AUTO_TEST_CASE( get_user_grp_owner )
{
	auto _usrgrp = lxr::FsUtils::osusrgrp("/bin/sh");
	//std::clog << _usrgrp.first << " " << _usrgrp.second << std::endl;
	BOOST_CHECK_EQUAL(_usrgrp.first, "root");
#ifdef __APPLE__
	BOOST_CHECK_EQUAL(_usrgrp.second, "wheel");
#endif
#ifdef __linux__
	BOOST_CHECK_EQUAL(_usrgrp.second, "root");
#endif
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
