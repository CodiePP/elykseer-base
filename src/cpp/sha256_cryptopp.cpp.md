declared in [Sha256](sha256.hpp.md)

```cpp

#if CRYPTOLIB == CRYPTOPP
Key256 Sha256::hash(std::string const & msg)
{
    return Sha256::hash(msg.c_str(), msg.size());
}

Key256 Sha256::hash(const char buffer[], int length)
{
    assert(256/8 == CryptoPP::SHA256::DIGESTSIZE);
    unsigned char digest[CryptoPP::SHA256::DIGESTSIZE];
    CryptoPP::SHA256 hash;
    hash.CalculateDigest( digest, (const unsigned char*)buffer, length );

    Key256 k;
    k.fromBytes(digest);
    return k;
}

Key256 Sha256::hash(boost::filesystem::path const & fpath)
{
    unsigned char digest[CryptoPP::SHA256::DIGESTSIZE];
    CryptoPP::SHA256 hash;

    Key256 k;
    unsigned char buf[1024];
    FILE *_f = fopen(fpath.c_str(), "r");
    if (_f) {
        while (!feof(_f)) {
            int nread = fread(buf, 1, 1024, _f);
            hash.Update(buf, nread);
        }
        hash.Final(digest);
        k.fromBytes(digest);
    }
    return k;
}
#endif

```
