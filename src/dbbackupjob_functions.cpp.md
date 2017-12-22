declared in [DbBackupJob](dbbackupjob.hpp.md)

```c++
void DbBackupJob::inStream(std::istream & ins)
{
    pugi::xml_document dbdoc;
    auto res = dbdoc.load(ins);
    if (!res) {
        std::clog << res.description() << std::endl;
        return;
    }
    auto dbroot = dbdoc.child("DbBackupJob");
    std::clog << "  host=" << dbroot.child_value("host") << "  user=" << dbroot.child_value("user") << "  date=" << dbroot.child_value("date") << std::endl;
    const std::string knodename = "Job";
    const std::string cPaths = "Paths";
    const std::string cPath = "Path";
    const std::string cFilters = "Filters";
    const std::string cFilterIncl = "include";
    const std::string cFilterExcl = "exclude";
    for (pugi::xml_node node: dbroot.children()) {
        if (knodename == node.name()) {
            DbJobDat dat;
            const std::string _name = node.attribute("name").value();
            //std::clog << "  job = " << _name << std::endl;
            for (pugi::xml_node node2: node.children()) {
                if (cPaths == node2.name()) {
                    for (pugi::xml_node node3: node2.children()) {
                        if (cPath == node3.name()) {
                            if (strncmp(node3.attribute("type").value(), "file", 4) == 0) {
                                dat._paths.push_back(node3.child_value());
                            }
                        }
                    }
                }
                else if (cFilters == node2.name()) {
                    for (pugi::xml_node node3: node2.children()) {
                        if (cFilterExcl == node3.name()) {
                            auto s = node3.child_value();
                            dat._strexcl.push_back(s);
                            dat._regexexcl.push_back(std::regex(s));
                        }
                        else if (cFilterIncl == node3.name()) {
                            auto s = node3.child_value();
                            dat._strincl.push_back(s);
                            dat._regexincl.push_back(std::regex(s));
                        }
                    }
                }
            }
            set(_name, dat); // add to db
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
        s.WriteLine("<DbBackupJob xmlns=\"http://spec.sbclab.com/lxr/v1.0\">")
        s.WriteLine("<library><name>{0}</name><version>{1}</version></library>", aname.Name, aname.Version.ToString())
        //s.WriteLine("<program><name>{0}</name><version>{1}</version></program>", xname.Name, xname.Version.ToString())
        s.WriteLine("<host>{0}</host>", System.Environment.MachineName)
        s.WriteLine("<user>{0}</user>", System.Environment.UserName)
        s.WriteLine("<date>{0}</date>", System.DateTime.Now.ToString("s"))
        this.idb.appValues (fun k (v : DbJobDat) ->
             s.WriteLine(@"  <Job name=""{0}"">", k)
             s.WriteLine("    <Paths>")
             v.paths |> Seq.iter(fun x ->
                 s.WriteLine(@"      <path type=""file"">{0}</path>", x) )
             s.WriteLine("    </Paths>")
             v.options.io.outStream s
             s.WriteLine("    <Filters>")
             v.regexexcl |> Seq.iter(fun x ->
                 s.WriteLine("    <exclude>{0}</exclude>", x) )
             v.regexincl |> Seq.iter(fun x ->
                 s.WriteLine("    <include>{0}</include>", x) )
             s.WriteLine("    </Filters>")
             s.WriteLine("  </Job>")
             )
        s.WriteLine("</DbBackupJob>")
        s.Flush()
```

```c++
void DbBackupJob::outStream(std::ostream & os) const
{
    os << "<?xml version=\\"1.0\\"?>" << std::endl;
    os << "<DbBackupJob xmlns=\\"http://spec.sbclab.com/lxr/v1.0\\">" << std::endl;
    os << "<library><name>" << Liz::name() << "</name><version>" << Liz::version() << "</version></library>" << std::endl;
    os << "<host>" << OS::hostname() << "</host>" << std::endl;
    os << "<user>" << OS::username() << "</user>" << std::endl;
    os << "<date>" << OS::timestamp() << "</date>" << std::endl;
    appValues([&os](std::string const & k, struct DbJobDat const & v) {
        os << "  <Job name=\\"" << k << "\\">" << std::endl;
        v._options.outStream(os);
        os << "    <Paths>" << std::endl;
        for (auto const & p : v._paths) {
            os << "    <path type=\\"file\\">" << p << "</path>" << std::endl;
        }
        os << "    </Paths>" << std::endl;
        os << "    <Filters>" << std::endl;
        for (auto const & p : v._strexcl) {
            os << "    <exclude>" << p << "</exclude>" << std::endl;
        }
        for (auto const & p : v._strincl) {
            os << "    <include>" << p << "</include>" << std::endl;
        }
        os << "    </Filters>" << std::endl;
        os << "  </Job>" << std::endl;
    });
    os << "</DbBackupJob>" << std::endl;
}
```
