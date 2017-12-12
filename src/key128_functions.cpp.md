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


```