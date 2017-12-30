```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/aes.hpp"
#include "lxr/key256.hpp"
#include "lxr/key128.hpp"
#include "sizebounded/sizebounded.hpp"
#include "sizebounded/sizebounded.ipp"

#include <iostream>
#include <fstream>
````

# Test suite: utAes

on class [Aes](../src/aes.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utAes )
```
## Test case: encrypt then decrypt
```cpp
BOOST_AUTO_TEST_CASE( encrypt_then_decrypt )
{
  const std::string msg = "all my precious data is save, so I will sleep fine!";

  lxr::Key256 _k;
  lxr::Key128 _iv;
  lxr::AesEncrypt _aesenc(_k, _iv);
  sizebounded<unsigned char, 1024> buf;
  buf.transform([&msg](const int i, const char c0)->char {
      if (i < msg.size()) { return msg[i]; }
      else { return '\0'; }
      });
  int lenc = 0;
  try {
    lenc = _aesenc.process(msg.size(), buf);
  } catch (std::exception & e) {
    std::clog << "exception " << e.what() << std::endl;
  }
  std::clog << "encrypted " << lenc << " bytes." << std::endl;
  lenc += _aesenc.finish(lenc, buf);
  std::clog << "finished: " << lenc << " bytes." << std::endl;

  // decrypt and compare to original message
  lxr::AesDecrypt _aesdec(_k, _iv);
  int ldec = 0;
  ldec = _aesdec.process(lenc, buf);
  ldec += _aesdec.finish(ldec, buf);

  std::string msg2 = buf.toString().substr(0, msg.size());

  BOOST_CHECK_EQUAL(msg, msg2);
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```

