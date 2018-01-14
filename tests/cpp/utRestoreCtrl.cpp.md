```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/restorectrl.hpp"

#include <iostream>
#include <fstream>
````

# Test suite: utRestoreCtrl

on class [BackupCtrl](../src/restorectrl.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utRestoreCtrl )
```
## Test case: startup invariants
```cpp
BOOST_AUTO_TEST_CASE( check_startup )
{
    lxr::Options _o;
    lxr::RestoreCtrl _ctrl(_o);

	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_in());
	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_out());
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
