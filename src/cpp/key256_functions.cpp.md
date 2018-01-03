declared in [Key256](key256.hpp.md)

```cpp

unsigned char const* Key256::bytes() const
{
    return _pimpl->_buffer.ptr();
}

void Key256::map(std::function<void(const int, const unsigned char)> f) const
{
    _pimpl->_buffer.map(f);
}

void Key256::transform(std::function<unsigned char(const int, const unsigned char)> f)
{
    _pimpl->_buffer.transform(f);
}

bool Key256::operator==(Key256 const & other) const
{
    bool res = true;
    for (int i=0; i<length()/8; i++) {
        res &= (_pimpl->_buffer[i] == other._pimpl->_buffer[i]);
        if (!res) { break; }
    }
    return res;
}


```
