declared in [Key128](key128.hpp.md)

initialize with random data

```cpp

Key128::Key128()
    : _pimpl(new Key128::pimpl)
{
    randomize();
}

Key128::~Key128() = default;

Key128::Key128(Key128 const & o)
    : Key128()
{
    _pimpl->_buffer = o._pimpl->_buffer;
}

Key128 & Key128::operator=(Key128 const & o)
{
    _pimpl->_buffer = o._pimpl->_buffer;
    return *this;
}

```