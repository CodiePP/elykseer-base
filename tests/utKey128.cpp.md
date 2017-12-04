```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/key128.hpp"
#include "sizebounded/sizebounded.ipp"

#include <iostream>
````

# Test suite: utKey128

on class [Key128](../src/key128.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utKey128 )
```
## Test case: verify that key is random
```cpp
BOOST_AUTO_TEST_CASE( new_key_is_random )
{
    lxr::Key128 k1, k2;
	//BOOST_CHECK_EQUAL(k1.toHex(), "abcdef0987654321");
	//BOOST_CHECK_EQUAL(k2.toHex(), "abcdef0987654321");
	BOOST_CHECK(k1.toHex() != k2.toHex());
}
```

## Test case: key length is 128 bits
```cpp
BOOST_AUTO_TEST_CASE( key_length )
{
    lxr::Key128 k;
	BOOST_CHECK_EQUAL(k.toHex().size(), 128 / 8 * 2);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
