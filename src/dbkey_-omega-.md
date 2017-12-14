```cpp

std::ostream & operator<<(std::ostream &os, lxr::DbKeyBlock const & block)
{
    os << "n=" << block._n << " key=" << block._key.toHex() << " iv=" << block._iv.toHex() << std::endl;
    return os;
}

} // namespace
```
