```cpp

/*
<fpaste ../../src/copyright.md>
*/

#pragma once

#include "lxr/streamio.hpp"
#include "boost/filesystem.hpp"
#include "pugixml.hpp"

#include <memory>
#include <iosfwd>

````

namespace [lxr](namespace.list) {

/*

```fsharp

type Options =
    class

    interface IStreamIO

        //override inStream : TextReader -> unit
        //override outStream : TextWriter -> unit

    (** ctor *)
    new : unit -> Options

    (** getters *)
    member nchunks : int
    member redundancy : int
    member fpath_chunks : string
    member fpath_db : string
    member isCompressed : bool
    member isDeduplicated : int

    (** setters *)
    member setNchunks : v:int -> unit
    member setRedundancy : v:int -> unit
    member setFpathChunks : v:string -> unit
    member setFpathDb : v:string -> unit
    member setCompression : v:bool -> unit
    member setDeduplication : v:int -> unit

    (** cast to the interface *)
    member io : IStreamIO

    end
```

*/

# class Options : public [StreamIO](streamio.hpp.md)

{

>public:

>static Options const & [current](options_functions.cpp.md)();

>static Options & [set](options_functions.cpp.md)();

>[Options](options_ctor.cpp.md)();

>[Options](options_ctor.cpp.md)(Options const &);

>Options & operator=(Options const &);

>[~Options](options_ctor.cpp.md)();

>void fromXML(pugi::xml_node&);

>virtual void inStream(std::istream&) override;

>virtual void outStream(std::ostream&) const override;

>int [nChunks](options_functions.cpp.md)() const;

>void [nChunks](options_functions.cpp.md)(int v);

>int [nRedundancy](options_functions.cpp.md)() const;

>void [nRedundancy](options_functions.cpp.md)(int v);

>bool [isCompressed](options_functions.cpp.md)() const;

>void [isCompressed](options_functions.cpp.md)(bool v);

>int [isDeduplicated](options_functions.cpp.md)() const;

>void [isDeduplicated](options_functions.cpp.md)(int v);

>boost::filesystem::path const & [fpathChunks](options_functions.cpp.md)() const;

>boost::filesystem::path & [fpathChunks](options_functions.cpp.md)();

>boost::filesystem::path const & [fpathMeta](options_functions.cpp.md)() const;

>boost::filesystem::path & [fpathMeta](options_functions.cpp.md)();

>protected:

>private:

>friend std::ostream & operator<<(std::ostream &os, Options const & opt);

>struct pimpl;

>std::unique_ptr&lt;pimpl&gt; _pimpl;

};

```cpp
std::ostream & operator<<(std::ostream &os, Options const & opt);

} // namespace
```
