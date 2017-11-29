declared in [Md5](md5.hpp.md)

```cpp

Key128 Md5::hash(std::string const & msg)
{
    return Md5::hash(msg.c_str(), msg.size());
}

Key128 Md5::hash(const char buffer[], int length)
{
    return Key128();
}

Key128 Md5::hash(boost::filesystem::path const & fpath)
{
    return Key128();
}

```