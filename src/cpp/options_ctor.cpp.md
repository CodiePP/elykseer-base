declared in [Options](options.hpp.md)

```cpp

Options::Options()
  :_pimpl(new pimpl)
{
}

Options::Options(Options const & o)
  :_pimpl(new pimpl)
{
  *_pimpl = *o._pimpl;
}

Options & Options::operator=(Options const & o)
{
  *_pimpl = *o._pimpl;
  return *this;
}

Options::~Options()
{
  if (_pimpl) {
    _pimpl.reset();
  }
}
```
