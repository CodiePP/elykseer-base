```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/randlist.hpp"

#include <iostream>
````

# Test suite: utRandList

on class [RandList](../src/randlist.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utRandList )
```
## Test case: generate list of integers
```cpp
BOOST_AUTO_TEST_CASE( list_integers )
{
	auto vs = lxr::RandList::Make(1, 100);
	int sum = 0;
	for (auto v : vs) {
		//std::clog << v << std::endl;
		sum += v;
	}
	BOOST_CHECK_EQUAL(vs.size(), 100);
	BOOST_CHECK_EQUAL(sum, 50*101);  // (1 + 100) + (2 + 99) + (3 + 98) ..
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
native