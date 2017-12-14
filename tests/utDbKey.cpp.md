```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/dbkey.hpp"

#include <iostream>
#include <fstream>
````

# Test suite: utDbKey

on class [DbKey](../src/dbkey.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utDbKey )
```
## Test case: set and get record
```cpp
BOOST_AUTO_TEST_CASE( set_get_record )
{
    const std::string aid1 = "94ffed38da8acee6f14be5af8a31d3b8015e008f9b30c7d12a58bfed57ba8d12";
	const std::string aid2 = "a31d3b8015e00894ffed38da8acee6f14be5af8f9b30c7d12a58bfed57ba8d12";
	
	lxr::DbKey _db;
	lxr::DbKeyBlock _block1;  _block1._n=64;
	lxr::DbKeyBlock _block2;  _block2._n=16;
	_db.set(aid1, _block1);
	_db.set(aid2, _block2);
	auto ob1 = _db.get(aid1);
	auto ob2 = _db.get(aid2);
	BOOST_CHECK(ob1);
	BOOST_CHECK(ob2);
	BOOST_CHECK_EQUAL(2, _db.count());
	BOOST_CHECK_EQUAL(16, ob2->_n);
	BOOST_CHECK_EQUAL(64, ob1->_n);
}
```

## Test case: output to XML file
```cpp
BOOST_AUTO_TEST_CASE( output_to_xml )
{
    const std::string aid1 = "94ffed38da8acee6f14be5af8a31d3b8015e008f9b30c7d12a58bfed57ba8d12";
	const std::string aid2 = "a31d3b8015e00894ffed38da8acee6f14be5af8f9b30c7d12a58bfed57ba8d12";
	
	lxr::DbKey _db;
	lxr::DbKeyBlock _block1;  _block1._n=64;
	lxr::DbKeyBlock _block2;  _block2._n=16;
	_db.set(aid1, _block1);
	_db.set(aid2, _block2);
	BOOST_CHECK_EQUAL(2, _db.count());
	const std::string _fpath = "/tmp/test_dbkey_1.xml";
	std::ofstream _outs; _outs.open(_fpath);
	_db.outStream(_outs);
}
```

## Test case: input from XML file
```cpp
BOOST_AUTO_TEST_CASE( input_from_xml )
{
    const std::string aid1 = "94ffed38da8acee6f14be5af8a31d3b8015e008f9b30c7d12a58bfed57ba8d12";
	const std::string aid2 = "a31d3b8015e00894ffed38da8acee6f14be5af8f9b30c7d12a58bfed57ba8d12";
	
	const std::string _fpath = "/tmp/test_dbkey_1.xml";
	lxr::DbKey _db;
	std::ifstream _ins; _ins.open(_fpath);
	_db.inStream(_ins);
	BOOST_CHECK_EQUAL(2, _db.count());
	BOOST_CHECK(_db.get(aid1));
	BOOST_CHECK(_db.get(aid2));
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
native