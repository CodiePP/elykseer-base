declared in [Key256](key256.hpp.md)

```cpp

void Key256::map(std::function<void(const int, const char)> f) const
{
    _pimpl->_buffer.map(f);
}

void Key256::transform(std::function<char(const int, const char)> f)
{
    _pimpl->_buffer.transform(f);
}

```