declared in [Assembly](assembly.hpp.md)

```cpp

Assembly::Assembly(int n)
  :_pimpl(new pimpl(n))
{ }

Assembly::Assembly(Key256 const & aid, int n)
  :_pimpl(new pimpl(aid, n))
{ }

Assembly::~Assembly() = default;

```
