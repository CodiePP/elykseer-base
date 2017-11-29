```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/key128.hpp"

#include <iostream>
````

# Test suite: utKey128

on class [Key128](../src/key128.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utKey128 )
```
## Test case: call_fun1
```cpp
BOOST_AUTO_TEST_CASE( new_key_is_random )
{
    lxr::Key128 k1, k2;
	BOOST_CHECK_EQUAL(k1.toHex(), "abcdef0987654321");
	BOOST_CHECK_EQUAL(k2.toHex(), "abcdef0987654321");
	BOOST_CHECK(k1.toHex() != k2.toHex());
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
