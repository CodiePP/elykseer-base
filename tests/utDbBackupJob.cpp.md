```cpp
#ifndef BOOST_ALL_DYN_LINK
#define BOOST_ALL_DYN_LINK
#endif

#include "boost/test/unit_test.hpp"

#include "lxr/dbbackupjob.hpp"

#include <iostream>
#include <fstream>
````

# Test suite: utDbBackupJob

on class [DbBackupJob](../src/dbbackupjob.hpp.md)

```cpp
BOOST_AUTO_TEST_SUITE( utDbBackupJob )
```
## Test case: set and get record
```cpp
BOOST_AUTO_TEST_CASE( set_get_record )
{
    const std::string name1 = "all my precious data";
	const std::string name2 = "the ledger";
	
	lxr::DbBackupJob _db;
    lxr::DbJobDat _job1;
    _job1._paths.push_back(std::make_pair("file","/home/me/Data/Performance.ods"));
    _job1._paths.push_back(std::make_pair("file","/home/me/Data/Base_Input.csv"));
    _job1._regexincl.push_back(std::regex(".*"));
	lxr::DbJobDat _job2;
    _job2._paths.push_back(std::make_pair("file","/home/me/Blocks.dat"));
    _job2._regexincl.push_back(std::regex(".*"));
	_db.set(name1, _job1);
	_db.set(name2, _job2);
	auto ob1 = _db.get(name1);
	auto ob2 = _db.get(name2);
	BOOST_CHECK(ob1);
	BOOST_CHECK(ob2);
	BOOST_CHECK_EQUAL(2, _db.count());
	BOOST_CHECK_EQUAL(1, ob2->_regexincl.size());
	BOOST_CHECK_EQUAL(0, ob1->_regexexcl.size());
}
```

## Test case: output to XML file
```cpp
BOOST_AUTO_TEST_CASE( output_to_xml )
{
    const std::string name1 = "all my precious data";
	const std::string name2 = "the ledger";
	
	lxr::DbBackupJob _db;
    lxr::DbJobDat _job1;
    _job1._paths.push_back(std::make_pair("file","/home/me/Data/Performance.ods"));
    _job1._paths.push_back(std::make_pair("file","/home/me/Data/Base_Input.csv"));
    _job1._regexincl.push_back(std::regex(".*"));
	lxr::DbJobDat _job2;
    _job2._paths.push_back(std::make_pair("file","/home/me/Blocks.dat"));
    _job2._regexincl.push_back(std::regex(".*"));
	_db.set(name1, _job1);
	_db.set(name2, _job2);
	BOOST_CHECK_EQUAL(2, _db.count());
	const std::string _fpath = "/tmp/test_dbbackupjob_1.xml";
	std::ofstream _outs; _outs.open(_fpath);
	_db.outStream(_outs);
}
```

## Test case: input from XML file
```cpp
BOOST_AUTO_TEST_CASE( input_from_xml )
{
    const std::string name1 = "all my precious data";
	const std::string name2 = "the ledger";
	
	const std::string _fpath = "/tmp/test_dbbackupjob_1.xml";
	lxr::DbBackupJob _db;
	std::ifstream _ins; _ins.open(_fpath);
	_db.inStream(_ins);
	BOOST_CHECK_EQUAL(2, _db.count());
	BOOST_CHECK(_db.get(name1));
	BOOST_CHECK(_db.get(name2));
}
```

```cpp
BOOST_AUTO_TEST_SUITE_END()
```
native