declared in [Options](options.hpp.md)

```cpp

Options Options::defaults()
{
  Options res;
  res.nRedundancy(1);
  res.nChunks(16);
  res.isCompressed(true);
  res.isDeduplicated(2);
  res.fpathMeta("/tmp");
  res.fpathChunks("/tmp");
  return res;
}

int Options::nChunks() const
{
  return _pimpl->_nchunks;
}

void Options::nChunks(int v)
{
  _pimpl->_nchunks = std::min(256, std::max(16, v));
}

int Options::nRedundancy() const
{
  return _pimpl->_nredundancy;
}

void Options::nRedundancy(int v)
{
  _pimpl->_nredundancy = v;
}

bool Options::isCompressed() const
{
  return _pimpl->_iscompressed;
}

void Options::isCompressed(bool v)
{
  _pimpl->_iscompressed = v;
}

int Options::isDeduplicated() const
{
  return _pimpl->_isdeduplicated;
}

void Options::isDeduplicated(int v)
{
  _pimpl->_isdeduplicated = v;
}

boost::filesystem::path Options::fpathChunks() const
{
  return _pimpl->_fpathchunks;
}

void Options::fpathChunks(boost::filesystem::path const & fp)
{
  _pimpl->_fpathchunks = fp;
}

boost::filesystem::path Options::fpathMeta() const
{
  return _pimpl->_fpathmeta;
}

void Options::fpathMeta(boost::filesystem::path const & fp)
{
  _pimpl->_fpathmeta = fp;
}

void Options::inStream(std::istream & i)
{
}

void Options::outStream(std::ostream & o) const
{
}


```
