```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/random.hpp"

#include <iostream>
````

# Test suite: utRandom

on class [Random](../src/random.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utRandom )
```
## Test case: call_fun1
```cpp
BOOST_AUTO_TEST_CASE( test_for_randomness )
{
    auto rng = new lxr::Random();
    uint32_t arr[5];
    for (int i=0; i<5; i++) {
        arr[i] = rng->random();
    }
    int32_t dff = 0L;
    for (int i=1; i<5; i++) {
        dff += arr[0];
        dff -= arr[i];
    }
	BOOST_CHECK(abs(dff) > 10000);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
