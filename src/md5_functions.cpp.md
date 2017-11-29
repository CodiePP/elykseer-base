declared in [Md5](md5.hpp.md)

```cpp

Key128 Md5::hash(std::string const & msg)
{
    return Md5::hash(msg.c_str(), msg.size());
}

Key128 Md5::hash(const char buffer[], int length)
{
    unsigned char digest[MD5_DIGEST_LENGTH];
    unsigned char *ret = MD5((const unsigned char*)buffer, length, digest);

    Key128 k;
    k.fromBytes((const char*)digest);
    return k;
}

/*
Key128 Md5::hash(boost::filesystem::path const & fpath)
{
    return Key128();
} */

```