```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/options.hpp"

#include <iostream>
#include <fstream>
````

# Test suite: utOptions

on class [Options](../src/options.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utOptions )
```
## Test case: export to and import from XML
```cpp
BOOST_AUTO_TEST_CASE( export_import_XML )
{
    lxr::Options _opts = lxr::Options::defaults();
    _opts.nChunks(17);
	const std::string _fpath = "/tmp/test_options_1.xml";
	std::ofstream _outs; _outs.open(_fpath);
    _opts.outStream(_outs);
    _outs.close();
    lxr::Options _opts2;
	std::ifstream _ins; _ins.open(_fpath);
    _opts2.inStream(_ins);
    BOOST_CHECK_EQUAL(17, _opts2.nChunks());
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
native