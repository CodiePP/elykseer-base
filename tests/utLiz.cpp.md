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
	BOOST_CHECK_EQUAL(msg, veritas);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
