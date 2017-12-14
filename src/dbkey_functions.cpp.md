declared in [DbKey](dbkey.hpp.md)

```cpp

void DbKey::inStream(std::istream & ins)
{
    pugi::xml_document dbdoc;
    auto res = dbdoc.load(ins);
    if (!res) {
        std::clog << res.description() << std::endl;
        return;
    }
    auto dbroot = dbdoc.child("DbKey");
    std::clog << "  host=" << dbroot.child_value("host") << "  user=" << dbroot.child_value("user") << "  date=" << dbroot.child_value("date") << std::endl;
    const std::string knodename = "Key";
    for (pugi::xml_node node: dbroot.children()) {
        if (knodename == node.name()) {
            DbKeyBlock block;
            block._n = node.attribute("n").as_int();
            block._iv.fromHex(node.attribute("iv").value());
            block._key.fromHex(node.child_value());
            const std::string _aid = node.attribute("aid").value();
            std::clog << "  aid=" << _aid << " block = " << block << std::endl;
            set(_aid, block); // add to db
        }
    }
}
```

```fsharp
    member this.outStream (s : TextWriter) =
        //let refl1 = Reflection.Assembly.GetCallingAssembly()
        let refl2 = Reflection.Assembly.GetExecutingAssembly()
        //let xname = refl1.GetName()
        let aname = refl2.GetName()
        s.WriteLine("<?xml version=\"1.0\"?>")
        s.WriteLine("<DbKey xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<library><name>{0}</name><version>{1}</version></library>", aname.Name, aname.Version.ToString())
        //s.WriteLine("<program><name>{0}</name><version>{1}</version></program>", xname.Name, xname.Version.ToString())
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k v ->
             let l = sprintf "  <Key aid=\"%s\" n=\"%d\" iv=\"%s\">%s</Key>" k v.n (Key.toHex v.iv.Length v.iv) (Key256.toHex v.key) in
             s.WriteLine(l))
        s.WriteLine("</DbKey>")
        s.Flush()
```

```cpp

void DbKey::outStream(std::ostream & os) const
{
    os << "<?xml version=\\"1.0\\"?>" << std::endl;
    os << "<DbKey xmlns=\\"http://spec.sbclab.com/lxr/v1.0\\">" << std::endl;
    os << "<library><name>" << Liz::name() << "</name><version>" << Liz::version() << "</version></library>" << std::endl;
    os << "<host>" << OS::hostname() << "</host>" << std::endl;
    os << "<user>" << OS::username() << "</user>" << std::endl;
    os << "<date>" << OS::timestamp() << "</date>" << std::endl;
    appValues([&os](std::string const & k, struct DbKeyBlock const & v) {
        os << "  <Key aid=\\"" << k << "\\" n=\\"" << v._n << "\\" iv=\\"" << v._iv.toHex() << "\\">" << v._key.toHex() << "</Key>" << std::endl;
    });
    os << "</DbKey>" << std::endl;
}
```