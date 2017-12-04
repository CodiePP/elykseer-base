declared in [Key](key.hpp.md)

helper functions
```cpp
int hex2int(char c) {
    if (c >= '0' && c <= '9') {
        return c - '0';
    } else if (c >= 'a' && c <= 'f') {
        return c - 'a' + 10;
    } else {
        return -1;
    }
}

char char2hex(char c) {
    if (c >= 0 && c <= 9) {
        return c + '0';
    } else if (c >= 10 && c <= 16) {
        return c + 'a' - 10;
    } else {
        return '?';
    }
}

int int2hex(char c) {
    char c1 = char2hex(c & 0x0f);
    char c2 = char2hex((c >> 4) & 0x0f);
    return (c2 << 8) | c1;
}
````


```cpp

char* Key::bytes() const
{
    return nullptr;
}

std::string Key::toHex() const
{
    char buf[2 * length() / 8];
    map([&buf](const int i, const char c) {
        int cc = int2hex(c);
        buf[2*i] = (cc >> 8) & 0xff;
        buf[2*i+1] = cc & 0xff;
    });
/*    for (int i=0; i<length() / 8; i++) {
        char c = _buffer.get()[i];
        int cc = int2hex(c);
        buf[2*i] = (cc >> 8) & 0xff;
        buf[2*i+1] = cc & 0xff;
    } */
    return std::string(buf, 2 * length() / 8);
}

void Key::fromHex(std::string const &k)
{

}

void Key::fromBytes(const char *buf)
{
/*    for (int i=0; i<length() / 8; i++) {
        _buffer.get()[i] = buf[i];
    } */
    transform([&buf](const int i, const char c) -> char {
        return buf[i];
    });
}

void Key::randomize()
{
    Random rng;
    uint32_t r = 0;
    transform([&rng,&r](const int i, const char c) -> char {
        if (i % 4 == 0) {
            r = rng.random(); }
        char c2 = r & 0xff;
        r = (r >> 8);
        return c2;
    });
}

```