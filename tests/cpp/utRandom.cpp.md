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
## Test case: subsequent random numbers have a significant difference
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
    delete rng;
}
```

## Test case: try hard to find an invalid number
```cpp
BOOST_AUTO_TEST_CASE( test_for_interval )
{
    auto rng = new lxr::Random();
    std::vector<int> f(201);
    for (int i=0; i<201; i++) { f[i] = 0; }
    constexpr int n = 1000000;
    for (int i=0; i<n; i++) {
        auto r = rng->random(200);
        //std::clog << r << " ";
        BOOST_CHECK( r >= 0 );
        BOOST_CHECK( r < 200 );
        ++f[(int)r];
    }
    BOOST_CHECK_EQUAL(f[200], 0);
    //for (int i=0; i<201; i++) {
    //    std::clog << i << " " << f[i] << std::endl;
    //}
    delete rng;
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
