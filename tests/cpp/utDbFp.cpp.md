```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/dbfp.hpp"
#include "lxr/key128.hpp"
#include "lxr/key256.hpp"

#include <iostream>
#include <fstream>
````

# Test suite: utDbFp

on class [DbFp](../src/dbfp.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utDbFp )
```
## Test case: set and get record
```cpp
BOOST_AUTO_TEST_CASE( set_get_record )
{
  const std::string fp1 = "/home/me/Documents/interesting.txt";
  const std::string fp2 = "/Data/photos/2001/motorcycle.jpeg";

  lxr::DbFp _db;
  lxr::DbFpDat _dat1 = lxr::DbFpDat::make(fp1);
  lxr::DbFpDat _dat2 = lxr::DbFpDat::make(fp2);
  _dat1._len = 1378; _dat2._len = 1749302;
  _dat1._osusr = "me"; _dat1._osgrp = "users";
  _dat2._osusr = "nobody"; _dat2._osgrp = "nobody";
  lxr::DbFpBlock _block1 {1, 193943, 0UL, (int)_dat1._len, (int)_dat1._len, false, {}, {}};
  _dat1._blocks.push_back(_block1);
  int idx = 1;
  uint64_t len = 0ULL;
  int bs = (1<<16) - 1;
  while (len < _dat2._len) {
    lxr::DbFpBlock _block2 {idx++, 7392+(int)len, len, std::min(bs,int(_dat2._len-len)), std::min(bs,int(_dat2._len-len)), false, {}, {}};
    _dat2._blocks.push_back(_block2);
    len += bs;
  }
  _db.set(fp1, _dat1);
  _db.set(fp2, _dat2);
  auto ob1 = _db.get(fp1);
  auto ob2 = _db.get(fp2);
  BOOST_CHECK(ob1);
  BOOST_CHECK(ob2);
  BOOST_CHECK_EQUAL(2, _db.count());
  BOOST_CHECK_EQUAL(_dat1._len, ob1->_len);
  BOOST_CHECK_EQUAL(_dat2._len, ob2->_len);
}
```

## Test case: output to XML file
```cpp
BOOST_AUTO_TEST_CASE( output_to_xml )
{
  const std::string fp1 = "/home/me/Documents/interesting.txt";
  const std::string fp2 = "/Data/photos/2001/motorcycle.jpeg";

  lxr::DbFp _db;
  lxr::DbFpDat _dat1 = lxr::DbFpDat::make(fp1);
  lxr::DbFpDat _dat2 = lxr::DbFpDat::make(fp2);
  _dat1._len = 1378; _dat2._len = 1749302;
  _dat1._osusr = "me"; _dat1._osgrp = "users";
  _dat2._osusr = "nobody"; _dat2._osgrp = "nobody";
  lxr::DbFpBlock _block1 {1, 193943, 0UL, (int)_dat1._len, (int)_dat1._len, false, {}, {}};
  _dat1._blocks.push_back(_block1);
  int idx = 1;
  uint64_t len = 0ULL;
  int bs = (1<<16) - 1;
  while (len < _dat2._len) {
    lxr::DbFpBlock _block2 {idx++, 7392+(int)len, len, std::min(bs,int(_dat2._len-len)), std::min(bs,int(_dat2._len-len)), false, {}, {}};
    _dat2._blocks.push_back(_block2);
    len += bs;
  }
  _db.set(fp1, _dat1);
  _db.set(fp2, _dat2);
  auto ob1 = _db.get(fp1);
  auto ob2 = _db.get(fp2);
  BOOST_CHECK(ob1);
  BOOST_CHECK(ob2);
  BOOST_CHECK_EQUAL(2, _db.count());
  const std::string _fpath = "/tmp/test_dbfp_1.xml";
  std::ofstream _outs; _outs.open(_fpath);
  _db.outStream(_outs);
}
```

## Test case: input from XML file
```cpp
BOOST_AUTO_TEST_CASE( input_from_xml )
{
  const std::string fp1 = "/home/me/Documents/interesting.txt";
  const std::string fp2 = "/Data/photos/2001/motorcycle.jpeg";

  const std::string _fpath = "/tmp/test_dbfp_1.xml";
  lxr::DbFp _db;
  std::ifstream _ins; _ins.open(_fpath);
  _db.inStream(_ins);
  BOOST_CHECK_EQUAL(2, _db.count());
  auto ob1 = _db.get(fp1);
  auto ob2 = _db.get(fp2);
  BOOST_CHECK(ob1);
  BOOST_CHECK(ob2);
  BOOST_CHECK_EQUAL(lxr::Md5::hash(fp1), ob1->_id);
  BOOST_CHECK_EQUAL(lxr::Md5::hash(fp2), ob2->_id);
}
```

## Test case: instantiate on real file
```cpp
BOOST_AUTO_TEST_CASE( instantiate_from_file )
{
  const std::string fp = "/bin/bash";

  auto _entry = lxr::DbFpDat::fromFile(fp);

  BOOST_CHECK( _entry._len > 0 );
  BOOST_CHECK_EQUAL( "0", _entry._osusr);
  BOOST_CHECK_EQUAL( "0", _entry._osgrp);
  BOOST_CHECK( _entry._osattr.size() > 0 );
  //std::clog << "mtime: " << _entry._osattr << std::endl;
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
native
