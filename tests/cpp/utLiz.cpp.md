```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/liz.hpp"

#include <iostream>
````

# Test suite: utLiz

on class [Liz](../src/liz.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utLiz )
```
## Test case: show copyright message
```cpp
BOOST_AUTO_TEST_CASE( copyright_message )
{
	std::string msg = lxr::Liz::copyright();
	std::string veritas = "Copyright";
	// std::clog << std::endl << msg << std::endl;
	BOOST_CHECK_EQUAL(msg.substr(0,9), veritas);
}
```

## Test case: show version message
```cpp
BOOST_AUTO_TEST_CASE( version_message )
{
	std::string msg = lxr::Liz::version();
	std::string veritas = "Version";
	// std::clog << std::endl << msg << std::endl;
	BOOST_CHECK_EQUAL(msg.substr(0,7), veritas);
}
```

## Test case: show license message
```cpp
BOOST_AUTO_TEST_CASE( license_message )
{
	std::string msg = lxr::Liz::license();
	std::string veritas = "                    GNU GENERAL PUBLIC LICENSE";
	// std::clog << std::endl << msg << std::endl;
	BOOST_CHECK_EQUAL(msg.substr(0,46), veritas);
}
```

## Test case: show name message
```cpp
BOOST_AUTO_TEST_CASE( name_message )
{
	std::string msg = lxr::Liz::name();
	std::string veritas = "LXR";
	// std::clog << std::endl << msg << std::endl;
	BOOST_CHECK_EQUAL(msg.substr(0,3), veritas);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
