declared in [FsUtils](fsutils.hpp.md)

## helper method to call an external UNIX command and capture its output

```cpp
#ifndef _WIN32
/*
std::string sh(std::string const & script) {
    constexpr int lenline = 256;
    std::array<char, lenline> _buffer;
    std::string _result;
    std::shared_ptr<FILE> _pipe(popen(script.c_str(), "r"), pclose);
    if (! _pipe) {
      throw std::runtime_error("popen() failed!"); }
    while (! feof(_pipe.get())) {
        if (fgets(_buffer.data(), lenline, _pipe.get()) != nullptr)
            _result += _buffer.data();
    }
    return _result;
} */
#endif
```


```cpp
std::string FsUtils::sep()
{
#ifdef _WIN32
    return "\\\\";
#else
    return "/";
#endif
}

const boost::filesystem::path FsUtils::cleanfp(boost::filesystem::path const & _fp)
{
#ifdef _WIN32
    return _fp.replace(":", ",drive");
#else
    return _fp;
#endif
}

const std::string FsUtils::fstem()
{
    const std::string _machine = OS::hostname();
    const std::string _user = OS::username();
    const std::string _ts = OS::timestamp();
    return "lxr_" + _machine + "_" + _user + "_" + _ts;
}

std::pair<const std::string, const std::string> FsUtils::osusrgrp(boost::filesystem::path const & fp)
{
#ifdef _WIN32
    #error not yet done
#else
    std::string _osusr = "error";
    std::string _osgrp = "error";
    struct stat _fi;
    stat(fp.c_str(), &_fi);  // Error check omitted
    constexpr int _blen = 512;
    char _buf[_blen];
    struct passwd _pw, *_retpw = NULL;
    if (getpwuid_r(_fi.st_uid, &_pw, _buf, _blen, &_retpw) == 0) {
        _osusr = _buf;
    }
    struct group _gr, *_retgr = NULL; //getgrgid(info.st_gid);
    if (getgrgid_r(_fi.st_gid, &_gr, _buf, _blen, &_retgr) == 0) {
        _osgrp = _buf;
    }
    return std::make_pair(_osusr, _osgrp);
#endif
}
```
