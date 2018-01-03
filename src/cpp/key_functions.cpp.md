declared in [Key](key.hpp.md)

helper functions
```cpp
int hex2int(unsigned char c) {
    if (c >= '0' && c <= '9') {
        return c - '0';
    } else if (c >= 'a' && c <= 'f') {
        return c - 'a' + 10;
    } else {
        return -1;
    }
}

unsigned char char2hex(unsigned char c) {
    if (c >= 0 && c <= 9) {
        return c + '0';
    } else if (c >= 10 && c <= 16) {
        return c + 'a' - 10;
    } else {
        return '?';
    }
}

int int2hex(unsigned char c) {
    unsigned char c1 = char2hex(c & 0x0f);
    unsigned char c2 = char2hex((c >> 4) & 0x0f);
    return (c2 << 8) | c1;
}
````


```cpp

unsigned char const* Key::bytes() const
{
    return nullptr;
}

std::string Key::toHex() const
{
    unsigned char buf[2 * length() / 8];
    map([&buf](const int i, const unsigned char c) {
        int cc = int2hex(c);
        buf[2*i] = (cc >> 8) & 0xff;
        buf[2*i+1] = cc & 0xff;
    });
    return std::string((char*)buf, 2 * length() / 8);
}

void Key::fromHex(std::string const &k)
{
    transform([&k](const int i, const unsigned char c) -> unsigned char {
        unsigned char c1 = k[i*2];
        unsigned char c2 = k[i*2+1];
        return (hex2int(c1)<<4 | hex2int(c2));
    });
}

void Key::fromBytes(unsigned char const *buf)
{
    transform([&buf](const int i, const unsigned char _c) -> unsigned char {
        unsigned char c = buf[i];
        return buf[i];
    });
}

void Key::randomize()
{
    Random rng;
    uint32_t r = 0;
    transform([&rng,&r](const int i, const unsigned char c) -> unsigned char {
        if (i % 4 == 0) {
            r = rng.random(); }
        unsigned char c2 = r & 0xff;
        r = (r >> 8);
        return c2;
    });
}

std::ostream & operator<<(std::ostream & os, Key const & k)
{
    os << k.toHex();
    return os;
}

```
