```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/key256.hpp"
#include "sizebounded/sizebounded.ipp"

#include <iostream>
````

# Test suite: utKey256

on class [Key256](../src/key256.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utKey256 )
```
## Test case: verify that key is random
```cpp
BOOST_AUTO_TEST_CASE( new_key_is_random )
{
    lxr::Key256 k1, k2;
	//BOOST_CHECK_EQUAL(k1.toHex(), "abcdef0987654321");
	//BOOST_CHECK_EQUAL(k2.toHex(), "abcdef0987654321");
	BOOST_CHECK(k1.toHex() != k2.toHex());
}
```

## Test case: key length is 256 bits
```cpp
BOOST_AUTO_TEST_CASE( key_length )
{
    lxr::Key256 k;
	BOOST_CHECK_EQUAL(k.toHex().size(), 256 / 8 * 2);
}
```

## Test case: key restored from string representation
```cpp
BOOST_AUTO_TEST_CASE( restore_key_from_string_representation )
{
	lxr::Key256 k1, k2;
	k2.fromHex(k1.toHex());
	BOOST_CHECK_EQUAL(k1, k2);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
