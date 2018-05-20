declared in [Options](options.hpp.md)

```cpp

static Options _options;

Options const & Options::current()
{
  return _options;
}

Options & Options::set()
{
  return _options;
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

boost::filesystem::path const & Options::fpathChunks() const
{
  return _pimpl->_fpathchunks;
}

boost::filesystem::path & Options::fpathChunks()
{
  return _pimpl->_fpathchunks;
}

boost::filesystem::path const & Options::fpathMeta() const
{
  return _pimpl->_fpathmeta;
}

boost::filesystem::path & Options::fpathMeta()
{
  return _pimpl->_fpathmeta;
}

void Options::inStream(std::istream & ins)
{
    pugi::xml_document dbdoc;
    auto res = dbdoc.load(ins);
    if (!res) {
        std::clog << res.description() << std::endl;
        return;
    }
    auto dbroot = dbdoc.child("Options");
    fromXML(dbroot);
}

void Options::fromXML(pugi::xml_node & dbroot)
{
    const std::string cMemory = "memory";
    const std::string cFPaths = "fpaths";
    const std::string comp = dbroot.child_value("compression");
    if (comp == "on" || comp == "ON") {
      isCompressed(true);
    } else {
      isCompressed(false);
    }
    for (pugi::xml_node node: dbroot.children()) {
        if (cMemory == node.name()) {
          nChunks(node.attribute("nchunks").as_int());
          nRedundancy(node.attribute("redundancy").as_int());
        }
        else if (cFPaths == node.name()) {
          fpathChunks() = node.child_value("chunks");
          fpathMeta() = node.child_value("meta");
        }
    }
}

void Options::outStream(std::ostream & os) const
{
  os << "<Options>" << std::endl;
  os << "  <memory nchunks=\\"" << _pimpl->_nchunks << "\\" redundancy=\\"" << _pimpl->_nredundancy << "\\" />" << std::endl;
  os << "  <compression>" << (_pimpl->_iscompressed?"on":"off") << "</compression>" << std::endl;
  os << "  <deduplication level=\\"" << _pimpl->_isdeduplicated << "\\" />" << std::endl;
  os << "  <fpaths>" << std::endl;
  os << "    <meta>" << _pimpl->_fpathmeta.native() << "</meta>" << std::endl;
  os << "    <chunks>" << _pimpl->_fpathchunks.native() << "</chunks>" << std::endl;
  os << "  </fpaths>" << std::endl;
  os << "</Options>" << std::endl;
}

```
