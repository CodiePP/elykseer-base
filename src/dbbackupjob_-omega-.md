```cpp

std::ostream & operator<<(std::ostream &os, lxr::DbJobDat const & dat)
{
    os << "job options =" << dat._options << " #paths=" << dat._paths.size() << std::endl;
    return os;
}

} // namespace
```
