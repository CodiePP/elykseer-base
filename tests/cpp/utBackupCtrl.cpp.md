```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/backupctrl.hpp"
#include "lxr/chunk.hpp"

#include <iostream>
#include <fstream>
````

# Test suite: utBackupCtrl

on class [BackupCtrl](../src/backupctrl.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utBackupCtrl )
```
## Test case: startup invariants
```cpp
BOOST_AUTO_TEST_CASE( check_startup )
{
    lxr::Options _o;
	_o.nChunks(21);
    lxr::BackupCtrl _ctrl(_o);

	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_in());
	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_out());
	BOOST_CHECK_EQUAL(lxr::Chunk::height * lxr::Chunk::width * 21, _ctrl.free());
}
```

## Test case: encrypt a file
```cpp
BOOST_AUTO_TEST_CASE( encrypt_file )
{
    lxr::Options _o;
	_o.isCompressed(true);
	_o.fpathChunks() = "/tmp/LXR";
	_o.fpathMeta() = "/tmp/meta";
    lxr::BackupCtrl _ctrl(_o);

	unsigned long _free0 = _ctrl.free();
	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_in());
	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_out());

	_ctrl.backup("/bin/bash");

	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_in());
	BOOST_CHECK_EQUAL(0UL, _ctrl.bytes_out());
	BOOST_CHECK(_ctrl.free() < _free0);
	
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
