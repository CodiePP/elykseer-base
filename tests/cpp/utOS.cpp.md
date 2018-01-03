```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/os.hpp"

#include <iostream>
````

# Test suite: utOS

on class [OS](../src/os.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utOS )
```
## Test case: show hostname
```cpp
BOOST_AUTO_TEST_CASE( get_hostname )
{
	std::string msg = lxr::OS::hostname();
	//std::clog << std::endl << msg << std::endl;
	BOOST_CHECK(! msg.empty());
}
```

## Test case: show username
```cpp
BOOST_AUTO_TEST_CASE( get_username )
{
	std::string msg = lxr::OS::username();
	//std::clog << std::endl << msg << std::endl;
	BOOST_CHECK(! msg.empty());
}
```

## Test case: show timestamp
```cpp
BOOST_AUTO_TEST_CASE( get_timestamp )
{
	std::string msg = lxr::OS::timestamp();
	//std::clog << std::endl << msg << std::endl;
	BOOST_CHECK(! msg.empty());
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
