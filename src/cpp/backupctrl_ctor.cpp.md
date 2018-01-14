declared in [BackupCtrl](backupctrl.hpp.md)

```cpp
BackupCtrl::BackupCtrl(Options const & o)
  : _pimpl(new pimpl(o))
{

}

BackupCtrl::~BackupCtrl()
{
  if (_pimpl) {
    _pimpl.reset();
  }
}
```
