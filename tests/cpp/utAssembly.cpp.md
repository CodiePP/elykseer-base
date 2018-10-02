```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"
#include "boost/contract.hpp"

#include "lxr/assembly.hpp"
#include "lxr/key256.hpp"
#include "lxr/key128.hpp"
#include "lxr/options.hpp"
#include "sizebounded/sizebounded.ipp"

#include <iostream>
#include <fstream>
#include <sstream>
````

# Test suite: utAssembly

on class [Assembly](../src/assembly.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utAssembly )
```

## Test case: state of assembly
```cpp
BOOST_AUTO_TEST_CASE( assembly_state_after_init )
{
  lxr::Assembly _a1(16);

  BOOST_CHECK(_a1.isWritable());
  BOOST_CHECK(! _a1.isReadable());
  BOOST_CHECK(! _a1.isEncrypted());
}
```

## Test case: state of assembly
```cpp
BOOST_AUTO_TEST_CASE( assembly_creation_failed )
{
  boost::contract::set_precondition_failure (
        [] (boost::contract::from where) {
            throw; // Re-throw (assertion_failure, user-defined, etc.).
        }
    );

  BOOST_CHECK_THROW(lxr::Assembly _a1(1), boost::contract::assertion_failure);
}
```

## Test case: access buffer denied if unencrypted
```cpp
BOOST_AUTO_TEST_CASE( access_buffer_denied )
{
  lxr::Assembly _a1(16);
  sizebounded<unsigned char, lxr::Assembly::datasz> buf;
  // cannot read from unencrypted buffer
  BOOST_CHECK_EQUAL(0, _a1.getData(0, 10, buf));
}
```

## Test case: access buffer if encrypted
```cpp
BOOST_AUTO_TEST_CASE( access_buffer_allowed )
{
  lxr::Assembly _a1(16);
  sizebounded<unsigned char, lxr::Assembly::datasz> buf;
  lxr::Key256 _k;
  lxr::Key128 _iv;
  BOOST_CHECK( _a1.encrypt(_k, _iv) );
  BOOST_CHECK_EQUAL(11, _a1.getData(0, 10, buf));
}
```

## Test case: encrypt then decrypt assembly
```cpp
BOOST_AUTO_TEST_CASE( assembly_encrypt_then_decrypt )
{
  const std::string msg = "all my precious data are save, so I will sleep fine!";

  lxr::Assembly _a1(16);
  sizebounded<unsigned char, lxr::Assembly::datasz> buf;
  buf.transform([&msg](int i, unsigned char c)->unsigned char {
      if (i < msg.size()) {
          return msg[i];
      } else {
          return '\\0';
      }
  });
  _a1.addData(msg.size(), buf);
  lxr::Key256 _k;
  lxr::Key128 _iv;
  BOOST_CHECK( _a1.encrypt(_k, _iv) );
 
  BOOST_CHECK(! _a1.isWritable());
  BOOST_CHECK(_a1.isReadable());
  BOOST_CHECK(_a1.isEncrypted());

  buf.transform([](int i, unsigned char c)->unsigned char {
      return '\\0';
  });
  BOOST_CHECK_EQUAL(10, _a1.getData(0,9,buf));
  BOOST_CHECK(buf.toString().substr(0,9) != msg.substr(0,9));

  BOOST_CHECK(  _a1.decrypt(_k, _iv) );
  BOOST_CHECK(! _a1.isWritable() );
  BOOST_CHECK(  _a1.isReadable() );
  BOOST_CHECK(! _a1.isEncrypted() );
  buf.transform([](int i, unsigned char c)->unsigned char {
      return '\\0';
  });
  BOOST_CHECK_EQUAL(10, _a1.getData(0,9,buf));
  BOOST_CHECK_EQUAL(buf.toString().substr(0,9), msg.substr(0,9));
}
```

## Test case: encrypt assembly then extract chunks
```cpp
BOOST_AUTO_TEST_CASE( assembly_encrypt_then_extract_chunks )
{
  const std::string msg = "0123456789abcdefghijklmnopqrstuvwxyz";

  const std::string outputpath = "/tmp/LXR";
  if (! boost::filesystem::exists(outputpath)) {
    boost::filesystem::create_directory(outputpath);
  }
  lxr::Options::set().fpathChunks() = outputpath;
  lxr::Assembly _a1(16);
  sizebounded<unsigned char, lxr::Assembly::datasz> buf;
  buf.transform([&msg](int i, unsigned char c)->unsigned char {
    return msg[i % msg.size()];
  });
  _a1.addData(buf.size(), buf);
  lxr::Key256 _k;
  lxr::Key128 _iv;
  BOOST_CHECK( _a1.encrypt(_k, _iv) );
  BOOST_CHECK( _a1.isEncrypted() );
  BOOST_CHECK( _a1.extractChunks() );

  auto const aid = _a1.aid();
  lxr::Assembly _a2(aid, 16);
  BOOST_CHECK( _a2.insertChunks() );
  BOOST_CHECK( _a2.isEncrypted() );
  BOOST_CHECK( _a2.decrypt(_k, _iv) );
  BOOST_CHECK(!_a2.isEncrypted() );

  buf.transform([](int i, unsigned char c)->unsigned char {
    return '\\0';
  });
  _a2.getData(0, buf.size()-1, buf);
  buf.transform([&msg](int i, unsigned char c)->unsigned char {
    if (msg[i % msg.size()] == c) {
        return '\\0'; }
    else {
        return '\\1'; }
  });
  int count = 0;
  for (int i=0; i<buf.size(); i++) {
      count += (int)buf[i];
  }
  BOOST_CHECK_EQUAL(0, count);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```

