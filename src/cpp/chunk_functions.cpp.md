declared in [Chunk](chunk.hpp.md)

```cpp

char Chunk::get(int pos) const
{
  return (*_pimpl->_buffer)[pos];
}

void Chunk::set(int pos, char val)
{
  (*_pimpl->_buffer)[pos] = val;
}

void Chunk::clear()
{
  _pimpl->_buffer->transform([](int i, char c) {
    return '\0';
  });
}

bool Chunk::fromFile(boost::filesystem::path const & fpath)
{
  return false;
}

bool Chunk::toFile(boost::filesystem::path const & fpath) const
{
  return false;
}

```
