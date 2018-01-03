```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "streamio.hpp"
#include <unordered_map>
#include <functional>
#include <experimental/optional>

````

namespace [lxr](namespace.list) {

/*

```fsharp


type IDb<'k, 'e> =

    inherit IStreamIO

    abstract member contains : 'k -> bool
    abstract member get : 'k -> 'e option
    abstract member set : 'k -> 'e -> unit
    abstract member count : int
    abstract member union : IDb<'k,'e> -> unit
    abstract member appKeys : ('k -> unit) -> unit
    abstract member appValues : ('k -> 'e -> unit) -> unit

```

*/

> template &lt;typename T, typename K = std::string&gt;

# class DbCtrl : public [StreamIO](streamio.hpp.md)

{

>public:

>virtual int count() const {
      return _map.size(); }

>virtual bool contains(K const & k) const {
      try { return (_map.at(k),true); } catch (...) { return false; }; }

>virtual std::experimental::optional&lt;T&gt; get(K const & k) {
```cpp
      try { T & r = _map.at(k); return r; } catch (...) { return {}; }; }
```

>virtual void set(K const & k, T const & v) {
      _map[k]=v; }

>virtual void unionWith(DbCtrl&lt;T,K&gt; const & other) {
```cpp
      other.appValues([this](K const & k, T const & v) {
        if (! this->contains(k)) {
          this->set(k, v); }
      });
    }
```

>virtual void appKeys(std::function&lt;void(K const &)&gt; f) const {
```cpp
      for (const auto & p : _map) {
        f(p.first); }
    }
```

>virtual void appValues(std::function&lt;void(K const &, T const &)&gt; f) const {
```cpp
      for (const auto & p : _map) {
        f(p.first, p.second); }
    }
```

>protected:

>DbCtrl() {}

>std::unordered_map&lt;K,T&gt; _map;

>private:

>DbCtrl(DbCtrl const &) = delete;

>DbCtrl & operator=(DbCtrl const &) = delete;

};

```cpp
} // namespace
```
