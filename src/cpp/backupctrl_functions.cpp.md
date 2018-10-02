declared in [BackupCtrl](backupctrl.hpp.md)

```cpp
void BackupCtrl::setReference()
{
 // TODO
}

void BackupCtrl::finalize()
{
    if (! _pimpl->_ass) { return; }
    _pimpl->_ass->extractChunks();
}

bool BackupCtrl::pimpl::renew_assembly()
{
    if (_ass) {
        // extract
        if (! _ass->extractChunks()) {
            return false;
        }
    }
    _ass.reset(new Assembly(_o.nChunks()));
    return true;
}

bool BackupCtrl::backup(boost::filesystem::path const & fp)
{
    if (! _pimpl->_ass) { return false; }
    if (! FileCtrl::fileExists(fp)) { return false; }

    uint64_t fsz = FileCtrl::fileSize(fp);
    sizebounded<unsigned char, Assembly::datasz> buffer;
    FILE *fptr = fopen(fp.native().c_str(), "r");
    if (! fptr) { return false; }

    // make DbFP entry
    auto dbentry = DbFpDat::fromFile(fp);
    dbentry._checksum = Sha256::hash(fp);

    int _bidx = 1;
    uint64_t _fpos = 0;
    while (! feof(fptr)) {
        size_t nread = fread((void*)buffer.ptr(), 64, Assembly::datasz / 64, fptr);
        int nwritten = _pimpl->_ass->addData(nread * 64, buffer);
        // what if nwritten < 64 bytes?
        auto dbblock = DbFpBlock(
            _bidx++,
            _pimpl->_ass->pos(),
            _fpos,
            // blen, clen
            nwritten, nwritten,
            false, // compressed?
            Md5::hash((const char *)buffer.ptr(), nwritten), // checksum
            _pimpl->_ass->aid()
            );

        if (nwritten != nread * 64) {
            if (! _pimpl->renew_assembly()) {
                return false;
            }
            int nwritten2 = _pimpl->_ass->addData(nread * 64 - nwritten, buffer, nwritten);
        }
    }
    fclose(fptr);

    _pimpl->_dbfp.set(fp.native(), dbentry);

    return true;
}

```
