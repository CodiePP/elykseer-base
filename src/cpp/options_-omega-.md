```cpp

std::ostream & operator<<(std::ostream &os, lxr::Options const & opt)
{
    os << "Options n=" << opt._pimpl->_nchunks << " r=" << opt._pimpl->_nredundancy << " compr=" << opt._pimpl->_iscompressed << " dedup=" << opt._pimpl->_isdeduplicated << " fpath chunks=" << opt._pimpl->_fpathchunks << " fpath db=" << opt._pimpl->_fpathmeta << std::endl;
    return os;
}

} // namespace
```