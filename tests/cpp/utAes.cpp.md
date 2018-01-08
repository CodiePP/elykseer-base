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
#include <sstream>
````

# Test suite: utAes

on class [Aes](../src/aes.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utAes )
```
## Test case: encrypt then decrypt a small buffer
```cpp
BOOST_AUTO_TEST_CASE( small_encrypt_then_decrypt )
{
  const std::string msg = "all my precious data are save, so I will sleep fine!";

  lxr::Key256 _k;
  lxr::Key128 _iv;
  lxr::AesEncrypt _aesenc(_k, _iv);
  sizebounded<unsigned char, lxr::Aes::datasz> buf;
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
  //std::clog << "encrypted " << lenc << " bytes." << std::endl;
  lenc += _aesenc.finish(lenc, buf);
  //std::clog << "finished: " << lenc << " bytes." << std::endl;

  // decrypt and compare to original message
  lxr::AesDecrypt _aesdec(_k, _iv);
  int ldec = 0;
  ldec = _aesdec.process(lenc, buf);
  ldec += _aesdec.finish(ldec, buf);

  std::string msg2 = buf.toString().substr(0, msg.size());

  BOOST_CHECK_EQUAL(msg, msg2);
}
```

## Test case: encrypt then decrypt a large buffer
```cpp
  
std::string encrypt_decrypt_test(std::string const & part, int counter)
{
  std::string msg;
  for (int i=0; i<counter; i++) {
    msg += part;
  }

  int tot_len = msg.size();
  int proc_len = 0;
  int last_len = 0;
  int tot_proc = 0;

  lxr::Key256 _k;
  lxr::Key128 _iv;
  lxr::AesEncrypt _aesenc(_k, _iv);
  sizebounded<unsigned char, lxr::Aes::datasz> buf;
  std::string cipher;
  int lenc = 0;

  while (proc_len < tot_len) {
    last_len = proc_len;
    // fill buffer
    buf.transform([&msg,&tot_len,&proc_len](const int i, const char c0)->char {
        if (proc_len < tot_len) { return msg[proc_len++]; }
        else { return '\0'; }
        });
    lenc = _aesenc.process(proc_len-last_len, buf);
    tot_proc += lenc;
    //std::clog << "encrypted " << lenc << " bytes." << std::endl;
    cipher += std::string((const char*)buf.ptr(), lenc);
  }
  lenc = _aesenc.finish(0, buf);
  tot_proc += lenc;
  //std::clog << "finished: " << lenc << " bytes => total " << tot_proc << " bytes." << std::endl;
  if (lenc > 0) {
    cipher += std::string((const char*)buf.ptr(), lenc); }

  // decrypt and compare to original message
  lxr::AesDecrypt _aesdec(_k, _iv);
  int ldec = 0;
  std::string cleartext;
  tot_len = cipher.size();
  proc_len = 0;
  last_len = 0;
  tot_proc = 0;
  while (proc_len < tot_len) {
    last_len = proc_len;
    // fill buffer
    buf.transform([&cipher,&tot_len,&proc_len](const int i, const char c0)->char {
        if (proc_len < tot_len) { return cipher[proc_len++]; }
        else { return '\0'; }
        });
    ldec = _aesdec.process(proc_len-last_len, buf);
    tot_proc += ldec;
    //std::clog << "decrypted " << ldec << " bytes." << std::endl;
    cleartext += std::string((const char*)buf.ptr(), ldec);
  }
  ldec = _aesdec.finish(0, buf);
  tot_proc += ldec;
  //std::clog << "finished: " << lenc << " bytes => total " << tot_proc << " bytes." << std::endl;
  if (ldec > 0) {
    cleartext += std::string((const char*)buf.ptr(), ldec); }

  return cleartext;
}

std::string s_repeat(std::string const & s0, int ct)
{
  std::ostringstream ss;
  while (ct-- > 0) {
    ss << s0;
  }
  return ss.str();
}

BOOST_AUTO_TEST_CASE( large_encrypt_then_decrypt )
{
  const std::string part = "all my precious data are save, so I will sleep fine!\\n";

  for (int k = 0; k < 99; k++) {
    std::string msg = s_repeat(part, k);
    std::string msg2 = encrypt_decrypt_test(part, k);
    BOOST_CHECK_EQUAL(msg, msg2);
  }
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```

