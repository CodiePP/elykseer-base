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
    lxr::Options & _opts = lxr::Options::set();
    _opts.nChunks(17);
    _opts.fpathChunks() = "/data/storage";
    _opts.fpathMeta() = "/mnt/secure";
	const std::string _fpath = "/tmp/test_options_1.xml";
	std::ofstream _outs; _outs.open(_fpath);
    _opts.outStream(_outs);
    _outs.close();
    lxr::Options & _opts1 = lxr::Options::set();
	std::ifstream _ins; _ins.open(_fpath);
    _opts1.inStream(_ins);
    auto const _opts2 = lxr::Options::current();
    BOOST_CHECK_EQUAL(17, _opts2.nChunks());
    BOOST_CHECK_EQUAL("/data/storage", _opts2.fpathChunks());
    BOOST_CHECK_EQUAL("/mnt/secure", _opts2.fpathMeta());
}
```

## Test case: set and get parameters
```cpp
BOOST_AUTO_TEST_CASE( setters_getters )
{
    lxr::Options & _opts = lxr::Options::set();
    _opts.nChunks(17);
    _opts.fpathChunks() = "/data/storage";
    _opts.fpathMeta() = "/mnt/secure";

    auto _opts2 = lxr::Options::current();
    BOOST_CHECK_EQUAL(17, _opts2.nChunks());
    BOOST_CHECK_EQUAL("/data/storage", _opts2.fpathChunks());
    BOOST_CHECK_EQUAL("/mnt/secure", _opts2.fpathMeta());

    const std::string _fpath = "/tmp/test_options.xml";
    std::ofstream _outs; _outs.open(_fpath);
    _opts.outStream(_outs);
}
```


<Options>
  <memory nchunks="16" redundancy="0" />
  <compression>on</compression>
  <deduplication level="2" />
  <fpaths>
    <meta>/tmp</meta>
    <chunks>/tmp/LXR</chunks>
  </fpaths>
</Options>

## Test case: read from XML
```cpp
BOOST_AUTO_TEST_CASE( readXML )
{
    lxr::Options & _opts = lxr::Options::set();
    _opts.nChunks(17);
    const std::string _fpath = "/tmp/test_options.xml";
    std::ifstream _ins; _ins.open(_fpath);
    _opts.inStream(_ins);

    BOOST_CHECK_EQUAL(17, _opts.nChunks());
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
native
