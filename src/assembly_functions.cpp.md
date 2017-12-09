declared in [Assembly](assembly.hpp.md)

```cpp

bool Assembly::encrypt(Key256 const & k, Key256 & iv)
{
  return false;
}

bool Assembly::decrypt(Key256 const & k)
{
  return false;
}

Key256 Assembly::mkChunkId(int idx) const
{
  return Key256();
}

bool Assembly::extractChunks() const
{
  return false;
}

bool Assembly::extractChunk(int idx) const
{
  return false;
}

bool Assembly::insertChunks()
{
  return false;
}

int Assembly::size() const
{
  return _pimpl->_n * Chunk::size;
}

int Assembly::free() const
{
  return size() - _pimpl->_pos;
}

bool Assembly::isWritable() const
{
  return (_pimpl->_state & writable) != 0;
}

bool Assembly::isEncrypted() const
{
  return (_pimpl->_state & encrypted) != 0;
}

bool Assembly::isReadable() const
{
  return (_pimpl->_state & (readable | writable)) != 0;
}

int Assembly::addData(int dlen, const sizebounded<char, datasz> & d)
{
  int wlen = 0;
  while (wlen < dlen) {
    int cnum = (wlen+_pimpl->_pos) % _pimpl->_n;   // 0 .. n-1  ; chunk
    int bidx = (wlen+_pimpl->_pos) / _pimpl->_n;   // 0 .. dlen/n ; pos in chunk
    _pimpl->_buffer[cnum][bidx] = d[wlen];
    ++wlen;
  }
  _pimpl->_pos += wlen;
  return wlen;
}

int Assembly::getData(int pos0, int pos1, sizebounded<char, datasz> & d) const
{
  return 0;
}


```
