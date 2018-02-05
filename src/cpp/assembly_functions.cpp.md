declared in [Assembly](assembly.hpp.md)

```c++
bool Assembly::encrypt(Key256 const & k, Key128 & ivout)
{
  // take ownership of buffer
  _pimpl->_state &= ~readable;
  _pimpl->_state &= ~writable;

  Key128 iv;
  AesEncrypt aesenc(k, iv);

  //
  sizebounded<unsigned char, Aes::datasz> buf;
  int pos = 0, sz = size();
  assert(sz % Aes::datasz == 0); // size is a multiple of Aes::datasz
  int lenc = 0, lastpos = 0;
  while (pos < sz) {
    int rlen = get_data(pos, Aes::datasz, buf);
    assert(rlen == Aes::datasz);
    pos += Aes::datasz;
    int lenc = aesenc.process(rlen, buf);
    set_data(lastpos, lenc, buf);
    lastpos += lenc;
  }
  lenc = aesenc.finish(0, buf);
  if (lenc > 0) {
    set_data(lastpos, lenc, buf); }

  ivout = iv;

  // new state
  _pimpl->_state |= encrypted;
  _pimpl->_state |= readable;

  return true;
}

bool Assembly::decrypt(Key256 const & k, Key128 const & iv)
{
  _pimpl->_state &= ~writable;
  _pimpl->_state &= ~readable;

  AesDecrypt aesdec(k, iv);

  sizebounded<unsigned char, Aes::datasz> buf;
  int pos = 0, sz = size();
  assert(sz % Aes::datasz == 0); // size is a multiple of Aes::datasz
  int ldec = 0, lastpos = 0;
  int rlen;
  while (pos < sz) {
    rlen = get_data(pos, Aes::datasz, buf);
    assert(rlen == Aes::datasz);
    pos += Aes::datasz;
    int ldec = aesdec.process(rlen, buf);
    set_data(lastpos, ldec, buf);
    lastpos += ldec;
  }
  ldec = aesdec.finish(0, buf);
  if (ldec > 0) {
    set_data(lastpos, ldec, buf); }

  // new state
  _pimpl->_state &= ~encrypted;
  _pimpl->_state |= readable;

  return true;
}
```

```fsharp
    let mkchunkid a n = 
        (said a) + (Printf.sprintf "%03dch%03d" n n)
        |> Sha256.hash_string
```
```c++
Key256 Assembly::mkChunkId(int idx) const
{
  constexpr int bsz = 256/8*2 + 3 + 2 + 3;
  char buf[bsz+1];
  snprintf(buf, bsz+1, "%s%03dch%03d__", _pimpl->said().c_str(), idx, idx);
  return Sha256::hash(buf, bsz);
}

boost::optional<const boost::filesystem::path> mk_chunk_path(Key256 const & cid0)
{
  auto const cid = cid0.toHex();
  auto fp = Options::current().fpathChunks();
  if (! FileCtrl::dirExists(fp)) { return {}; }
  fp /= cid.substr(62,2);
  if (! FileCtrl::dirExists(fp)) {
    if (! boost::filesystem::create_directory(fp)) { return {}; }
  }
  fp /= cid;
  fp.replace_extension(".lxr");
  return fp;
}

bool Assembly::extractChunks() const
{
  if (! isEncrypted()) { return false; }

  for (int n = 0; n < _pimpl->_n; n++) {
    if (extractChunk(n) == false) {
      return false;
    }
  }
  return true;
}

bool Assembly::extractChunk(int cnum) const
{
  auto fp = mk_chunk_path(mkChunkId(cnum));
  if (! fp) { return false; }
  return _pimpl->_chunks[cnum].toFile(*fp);
}

bool Assembly::insertChunks()
{
  if (! isWritable()) { return false; }
  _pimpl->_state &= ~writable;
  _pimpl->_state &= ~readable;

  for (int n = 0; n < _pimpl->_n; n++) {
    if (insertChunk(n) == false) {
      return false;
    }
  }

  _pimpl->_state |= encrypted;
  return true;
}

bool Assembly::insertChunk(int cnum)
{
  auto fp = mk_chunk_path(mkChunkId(cnum));
  if (! fp) { return false; }
  return _pimpl->_chunks[cnum].fromFile(*fp);
}

Key256 const Assembly::aid() const
{
  return _pimpl->_aid;
}

std::string const Assembly::said() const
{
  return _pimpl->said();
}

int Assembly::size() const
{
  return _pimpl->_n * Chunk::size;
}

uint32_t Assembly::pos() const
{
  return _pimpl->_pos;
}

uint32_t Assembly::free() const
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
  return (_pimpl->_state & readable) != 0;
}

int Assembly::addData(int dlen, const sizebounded<unsigned char, datasz> & d, int p0)
{
  if (! isWritable()) { return 0; }
  int wlen = set_data(_pimpl->_pos, dlen, d, p0);
  _pimpl->_pos += wlen;
  return wlen;
}

int Assembly::get_data(int pos, int dlen, sizebounded<unsigned char, datasz> & d) const
{
  if (dlen > datasz) { return 0; }
  if (pos < 0) { return 0; }
  if (pos + dlen > size()) { return 0; }

  int rlen = 0;
  while (rlen < dlen) {
    int cnum = (rlen+pos) % _pimpl->_n;   // 0 .. n-1  ; chunk number
    int bidx = (rlen+pos) / _pimpl->_n;   // 0 .. dlen/n ; pos in chunk
    d[rlen++] = _pimpl->_chunks[cnum].get(bidx);
  }
  return rlen;
}

int Assembly::set_data(int pos, int dlen, sizebounded<unsigned char, datasz> const & d, int p0)
{
  if (dlen + p0 > datasz) { return 0; }
  if (pos < 0) { return 0; }
  if (pos + dlen > size()) { return 0; }

  int wlen = 0;
  while (wlen < dlen) {
    int cnum = (wlen+pos) % _pimpl->_n;   // 0 .. n-1  ; chunk number
    int bidx = (wlen+pos) / _pimpl->_n;   // 0 .. dlen/n ; pos in chunk
    _pimpl->_chunks[cnum].set(bidx, d[p0 + wlen]);
    wlen++;
  }
  return wlen;
}

int Assembly::getData(int pos0, int pos1, sizebounded<unsigned char, datasz> & d) const
{
  if (! isReadable()) { return 0; }
  if (pos1 <= pos0) { return 0; }
  if (pos0 < 0) { return 0; }

  return get_data(pos0, pos1 - pos0 + 1, d);
}

```
