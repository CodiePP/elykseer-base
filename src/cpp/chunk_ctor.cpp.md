declared in [Chunk](chunk.hpp.md)

```cpp

Chunk::Chunk()
  :_pimpl(new pimpl)
{
  _pimpl->_buffer = std::make_shared<sizebounded<char, Chunk::size>>();
}

Chunk::Chunk(std::shared_ptr<sizebounded<char, Chunk::size>> buf)
  :_pimpl(new pimpl)
{
  _pimpl->_buffer = buf;
}

```
