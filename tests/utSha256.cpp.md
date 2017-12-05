```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/sha256.hpp"

#include <iostream>
````

# Test suite: utSha256

on class [Sha256](../src/sha256.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utSha256 )
```
## Test case: compare message digest to known one
```cpp
BOOST_AUTO_TEST_CASE( message_digest )
{
	std::string msg = "the cleartext message, I am.";
	std::string sha256 = "b27cd6b9a45d9aaa28c2319f33721ea5e8531b978a25b9c52993b75d5e90ff96";
	BOOST_CHECK_EQUAL(lxr::Sha256::hash(msg).toHex(), sha256);
}
```

## Test case: compute checksum over file
```cpp
BOOST_AUTO_TEST_CASE( file_checksum )
{
	boost::filesystem::path fp = "/bin/sh";
#ifdef __APPLE__
	std::string sha256 = "b494b48b6fe9626e6f10ea402dd34e8885a5e3a5de88fd070c2a2ec0643cef10";
#else 
#ifdef __linux__
	std::string sha256 = "8cc3e6ea29b4fa72578418757ca4bc01e2b448d1a9a4af014e2527a04dafb8b8";
#else
#error Where are we?
#endif
#endif
	BOOST_CHECK_EQUAL(lxr::Sha256::hash(fp).toHex(), sha256);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
