declared in [BackupCtrl](backupctrl.hpp.md)

```cpp
uint32_t BackupCtrl::free() const
{
    if (_pimpl->_ass) {
        return _pimpl->_ass->free();
    }
    return 0;
}

uint64_t BackupCtrl::bytes_in() const
{
    return _pimpl->trx_in;
}

uint64_t BackupCtrl::bytes_out() const
{
    return _pimpl->trx_out;
}

std::time_t BackupCtrl::time_encrypt() const
{
    return 0; // TODO
}

std::time_t BackupCtrl::time_extract() const
{
    return 0; // TODO
}

std::time_t BackupCtrl::time_write() const
{
    return 0; // TODO
}
```
