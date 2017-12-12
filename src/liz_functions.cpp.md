declared in [Liz](liz.hpp.md)

```cpp

const bool Liz::verify()
{
    return false;
}

const int Liz::daysLeft()
{
    return 0;
}

const std::string Liz::license()
{
    return "";
}

std::string decodeb64(std::string const & _enc)
{
    base64::decoder decoder;
    char *plaintext = new char[decoder._buffersize];
    std::string res;
    int pos = 0;
    int olen = _enc.size();
    const char *encbuf = _enc.c_str();
    while (olen > 0) {
        int textlen = decoder.decode(encbuf+pos, std::min(decoder._buffersize, olen), plaintext);
        res += std::string(plaintext, textlen);
        olen -= decoder._buffersize;
        pos += decoder._buffersize;
    }
    return res;
}

const std::string Liz::copyright()
{
    static std::string _enc = "Q29weXJpZ2h0IChjKSAyMDE3IEFsZXhhbmRlciBEaWVtYW5kCg==";
    return decodeb64(_enc);
}

const std::string Liz::version()
{
    return "";
}

const std::string Liz::name()
{
    return "";
}

```