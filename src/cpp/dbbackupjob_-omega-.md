```cpp

std::ostream & operator<<(std::ostream &os, lxr::DbJobDat const & dat)
{
    os << "job options =" << Options::current() << " #paths=" << dat._paths.size() << std::endl;
    return os;
}

} // namespace
```
