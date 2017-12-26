declared in [Assembly](assembly.hpp.md)

```c++
bool Assembly::encrypt(Key256 const & k, Key256 & iv)
{
  // take ownership of buffer
  _pimpl->_state &= ~writable;

  // 
  sizebounded<char, datasz> buf;
  char text0[datasz], text1[datasz];
  int pos = 0, sz = size();
  while (pos < sz) {
    int rlen = getData(pos, pos + datasz - 1, buf);
    pos += datasz;
    buf.map([&text0](const int i, const char c)->void {
      text0[i] = c;
    });
    // call AES encryption

  }

  // new state
  _pimpl->_state |= encrypted;

  return false;
}

bool Assembly::decrypt(Key256 const & k)
{
  // ...

  // new state
  _pimpl->_state &= ~encrypted;
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
  return (_pimpl->_state & writable & ~encrypted) != 0;
}

bool Assembly::isEncrypted() const
{
  return (_pimpl->_state & encrypted) != 0;
}

bool Assembly::isReadable() const
{
  return (_pimpl->_state & (readable | writable) & ~encrypted) != 0;
}

int Assembly::addData(int dlen, const sizebounded<char, datasz> & d)
{
  if (! isWritable()) { return 0; }
  int wlen = set_data(_pimpl->_pos, dlen, d);
  _pimpl->_pos += wlen;
  return wlen;
}

int Assembly::set_data(int pos, int dlen, const sizebounded<char, datasz> & d)
{
  if (dlen > datasz) { return 0; }
  if (pos < 0) { return 0; }
  if (pos + dlen >= size()) { return 0; }

  int wlen = 0;
  while (wlen < dlen) {
    int cnum = (wlen+pos) % _pimpl->_n;   // 0 .. n-1  ; chunk number
    int bidx = (wlen+pos) / _pimpl->_n;   // 0 .. dlen/n ; pos in chunk
    _pimpl->_buffer[cnum][bidx] = d[wlen++];
  }
  return wlen;
}

int Assembly::getData(int pos0, int pos1, sizebounded<char, datasz> & d) const
{
  if (! isReadable()) { return 0; }

  if (pos1 <= pos0) { return 0; }
  int dlen = pos1 - pos0 + 1;
  int rlen = 0;
  while (rlen < dlen) {
    int cnum = (rlen+pos0) % _pimpl->_n;   // 0 .. n-1  ; chunk
    int bidx = (rlen+pos0) / _pimpl->_n;   // 0 .. dlen/n ; pos in chunk
    d[rlen++] = _pimpl->_buffer[cnum][bidx];
  }
  return rlen;
}

```
