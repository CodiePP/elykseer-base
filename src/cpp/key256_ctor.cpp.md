declared in [Key256](key256.hpp.md)

initialize with random data

```cpp

Key256::Key256()
    : Key(), _pimpl(new Key256::pimpl)
{
    randomize();
}

Key256::~Key256() = default;

Key256::Key256(Key256 const & o)
    : Key256()
{
    _pimpl->_buffer = o._pimpl->_buffer;
}

Key256 & Key256::operator=(Key256 const & o)
{
    _pimpl->_buffer = o._pimpl->_buffer;
    return *this;
}

```