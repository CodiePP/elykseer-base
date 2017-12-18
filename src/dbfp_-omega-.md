```cpp

std::ostream & operator<<(std::ostream &os, lxr::DbFpDat const & dat)
{
    os << "id=" << dat._id.toHex() << " len=" << dat._len << std::endl;
    return os;
}

} // namespace
```
