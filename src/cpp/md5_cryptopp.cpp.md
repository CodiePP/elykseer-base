declared in [Md5](md5.hpp.md)

```cpp

#if CRYPTOLIB == CRYPTOPP
Key128 Md5::hash(std::string const & msg)
{
    return Md5::hash(msg.c_str(), msg.size());
}

Key128 Md5::hash(const char buffer[], int length)
{
    assert(128/8 == CryptoPP::Weak::MD5::DIGESTSIZE);
    unsigned char digest[CryptoPP::Weak::MD5::DIGESTSIZE];
    CryptoPP::Weak::MD5 hash;
    hash.CalculateDigest(digest, (unsigned char const *)buffer, length);

    Key128 k;
    k.fromBytes(digest);
    return k;
}
#endif

```
