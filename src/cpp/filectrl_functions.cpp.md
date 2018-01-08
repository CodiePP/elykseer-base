declared in [FileCtrl](filectrl.hpp.md)

```cpp

std::string FileCtrl::fileDate(boost::filesystem::path const & fp)
{
    return "";
}

std::time_t FileCtrl::fileLastWriteTime(boost::filesystem::path const & fp)
{
    return boost::filesystem::last_write_time(fp);
}

uint64_t FileCtrl::fileSize(boost::filesystem::path const & fp)
{
    return boost::filesystem::file_size(fp);
}

bool FileCtrl::fileExists(boost::filesystem::path const & fp)
{
    return boost::filesystem::exists(fp);
}

bool FileCtrl::isFileReadable(boost::filesystem::path const & fp)
{
    try {
        auto s = boost::filesystem::status(fp);
        return boost::filesystem::is_regular_file(s);
    } catch (...) {}
    return false;
}

bool FileCtrl::dirExists(boost::filesystem::path const & fp)
{
    try {
        auto s = boost::filesystem::status(fp);
        return boost::filesystem::is_directory(s);
    } catch (...) {}
    return false;
}

std::vector<boost::filesystem::path> FileCtrl::fileListRecursive(boost::filesystem::path const & fp)
{
    std::vector<boost::filesystem::path> res;
    boost::filesystem::directory_iterator _pit{fp};
    while (_pit != boost::filesystem::directory_iterator{}) {
        auto fp2 = *_pit++;
        if (dirExists(fp2)) {
            auto dsub = fileListRecursive(fp2);
            res.reserve( res.size() + dsub.size() );
            res.insert( res.end(), dsub.begin(), dsub.end() );
        } else {
            res.push_back(fp2);
        }
    }
    return res;
}

```