declared in [Key128](key128.hpp.md)

```cpp

void Key128::map(std::function<void(const int, const char)> f) const
{
    _pimpl->_buffer.map(f);
}

void Key128::transform(std::function<char(const int, const char)> f)
{
    _pimpl->_buffer.transform(f);
}

bool Key128::operator==(Key128 const & other) const
{
    bool res = true;
    for (int i=0; i<length()/8; i++) {
        res &= (_pimpl->_buffer[i] == other._pimpl->_buffer[i]);
        if (!res) { break; }
    }
    return res;
}


```