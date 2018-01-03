declared in [Sha256](sha256.hpp.md)

```cpp

#if CRYPTOLIB == OPENSSL
Key256 Sha256::hash(std::string const & msg)
{
    return Sha256::hash(msg.c_str(), msg.size());
}

Key256 Sha256::hash(const char buffer[], int length)
{
    unsigned char digest[SHA256_DIGEST_LENGTH];
    unsigned char *ret = SHA256((const unsigned char*)buffer, length, digest);

    Key256 k;
    k.fromBytes(digest);
    return k;
}

Key256 Sha256::hash(boost::filesystem::path const & fpath)
{
    unsigned char digest[SHA256_DIGEST_LENGTH];
    SHA256_CTX ctx;
    SHA256_Init(&ctx);

    Key256 k;
    unsigned char buf[1024];
    FILE *_f = fopen(fpath.c_str(), "r");
    if (_f) {
        while (!feof(_f)) {
            int nread = fread(buf, 1, 1024, _f);
            SHA256_Update(&ctx, buf, nread);
        }
        SHA256_Final(digest, &ctx);
        k.fromBytes(digest);
    }
    return k;
}
#endif

```
