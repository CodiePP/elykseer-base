declared in [Chunk](chunk.hpp.md)

```cpp

Chunk::Chunk()
  :_pimpl(new pimpl)
{
  _pimpl->_buffer = std::make_shared<sizebounded<unsigned char, Chunk::size>>();
}

Chunk::Chunk(std::shared_ptr<sizebounded<unsigned char, Chunk::size>> buf)
  :_pimpl(new pimpl)
{
  _pimpl->_buffer = buf;
}

Chunk::~Chunk() = default;

```
