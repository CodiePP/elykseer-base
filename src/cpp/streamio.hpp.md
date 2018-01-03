```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include <iosfwd>

````

namespace [lxr](namespace.list) {

/*

```fsharp

type IStreamIO =

    abstract member inStream : XmlTextReader -> unit
    abstract member outStream : TextWriter -> unit
```

*/

# class StreamIO

{

>public:

>virtual void inStream(std::istream &) = 0;

>virtual void outStream(std::ostream &) const = 0;

>protected:

>StreamIO() {}

>private:

>//StreamIO(StreamIO const &) = delete;

>//StreamIO & operator=(StreamIO const &) = delete;

};

```cpp
} // namespace
```
