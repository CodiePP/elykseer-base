declared in [DbFp](dbfp.hpp.md)

```c++

DbFpBlock::DbFpBlock(int i,int a,uint64_t f,int bl,int cl,bool c,Key128&& chk,Key256&& aid)
  : _idx(i), _apos(a)
  , _fpos(f), _blen(bl)
  , _clen(cl), _compressed(c)
  , _checksum(chk), _aid(aid)
{
}

```

```c++
void DbFp::inStream(std::istream & ins)
{
    pugi::xml_document dbdoc;
    auto res = dbdoc.load(ins);
    if (!res) {
        std::clog << res.description() << std::endl;
        return;
    }
    auto dbroot = dbdoc.child("DbFp");
    std::clog << "  host=" << dbroot.child_value("host") << "  user=" << dbroot.child_value("user") << "  date=" << dbroot.child_value("date") << std::endl;
    const std::string knodename = "Fp";
    for (pugi::xml_node node: dbroot.children()) {
        if (knodename == node.name()) {
            DbFpDat dat;
            dat._id.fromHex(node.attribute("id").value());
            const std::string _fp = node.attribute("fp").value();
            std::clog << "  fp=" << _fp << " id = " << dat._id.toHex() << std::endl;
            for (pugi::xml_node node2: node.children()) {
                if ("Fblock" == node2.name()) {
                    DbFpBlock block;
                    block._aid.fromHex(node2.child_value());
                    block._idx = node2.attribute("idx").as_int();
                    block._apos = node2.attribute("apos").as_int();
                    block._fpos = node2.attribute("fpos").as_ullong();
                    block._blen = node2.attribute("blen").as_int();
                    block._clen = node2.attribute("clen").as_int();
                    block._compressed = node2.attribute("compressed").as_bool();
                    block._checksum.fromHex(node2.attribute("chksum").value());
                    dat._blocks.push_back(block);
                }
                else if ("Fattrs" == node2.name()) {
                    dat._osusr = node2.child_value("osusr");
                    dat._osgrp = node2.child_value("osgrp");
                    const char *_len = node2.child_value("length");
                    dat._len = atol(_len);
                    dat._osattr = node2.child_value("last");
                    dat._checksum.fromHex(node2.child_value("chksum"));
                }
            }
            
            set(_fp, dat); // add to db
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
        s.WriteLine("<DbFp xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<library><name>{0}</name><version>{1}</version></library>", aname.Name, aname.Version.ToString())
        //s.WriteLine("<program><name>{0}</name><version>{1}</version></program>", xname.Name, xname.Version.ToString())
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k v ->
             let l1 = sprintf "  <Fp fp=\"%s\" id=\"%s\">" k (Key128.toHex v.id)
             s.WriteLine(l1)
             let l2 = sprintf "     <Fattrs><osusr>%s</osusr><osgrp>%s</osgrp><length>%d</length><last>%s</last><chksum>%s</chksum></Fattrs>" v.osusr v.osgrp v.len v.osattr (Key256.toHex v.checksum)
             s.WriteLine(l2)
             for b in v.blocks do
                 let l2 = sprintf "    <Fblock idx=\"%d\" apos=\"%d\" fpos=\"%d\" blen=\"%d\" clen=\"%d\" compressed=\"%s\" chksum=\"%s\">%s</Fblock>" b.idx b.apos b.fpos b.blen b.clen (if b.compressed then "1" else "0") (Key128.toHex b.checksum ) (Key256.toHex b.aid)
                 s.WriteLine(l2)
             s.WriteLine("  </Fp>")
         )
        s.WriteLine("</DbFp>")
        s.Flush()
```

```cpp

void DbFp::outStream(std::ostream & os) const
{
    os << "<?xml version=\\"1.0\\"?>" << std::endl;
    os << "<DbFp xmlns=\\"http://spec.sbclab.com/lxr/v1.0\\">" << std::endl;
    os << "<library><name>" << Liz::name() << "</name><version>" << Liz::version() << "</version></library>" << std::endl;
    os << "<host>" << OS::hostname() << "</host>" << std::endl;
    os << "<user>" << OS::username() << "</user>" << std::endl;
    os << "<date>" << OS::timestamp() << "</date>" << std::endl;
    appValues([&os](std::string const & k, struct DbFpDat const & v) {
        os << "  <Fp fp=\\"" << k << "\\" id=\\"" << v._id.toHex() << "\\">" << std::endl;
        os << "    <Fattrs><osusr>" << v._osusr << "</osusr><osgrp>" << v._osgrp << "</osgrp><length>" << v._len << "</length><last>" << v._osattr << "</last><chksum>" << v._checksum.toHex() << "</chksum></Fattrs>" << std::endl;
        for (const auto & b : v._blocks) {
            os << "    <Fblock idx=\\"" << b._idx << "\\" apos=\\"" << b._apos << "\\" fpos=\\"" << b._fpos << "\\" blen=\\"" << b._blen << "\\" clen=\\"" << b._clen << "\\" compressed=\\"" << b._compressed << "\\" chksum=\\"" << b._checksum.toHex() << "\\">" << b._aid.toHex() << "</Fblock>" << std::endl;
        }
        os << "  </Fp>" << std::endl;
    });
    os << "</DbFp>" << std::endl;
}
```
