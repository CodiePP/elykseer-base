declared in [Md5](md5.hpp.md)

```cpp

#if CRYPTOLIB == OPENSSL
Key128 Md5::hash(std::string const & msg)
{
    return Md5::hash(msg.c_str(), msg.size());
}

Key128 Md5::hash(const char buffer[], int length)
{
    unsigned char digest[MD5_DIGEST_LENGTH];
    unsigned char *ret = MD5((unsigned char const *)buffer, length, digest);

    Key128 k;
    k.fromBytes(digest);
    return k;
}
#endif

```
