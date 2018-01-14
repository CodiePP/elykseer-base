declared in [Aes](aes.hpp.md)

```c++

#if CRYPTOLIB == CRYPTOPP
struct Aes::pimpl {
    //pimpl() {};
    std::unique_ptr<CryptoPP::SymmetricCipher> _cipher;
    std::unique_ptr<CryptoPP::StreamTransformationFilter> _filter;
    int _len {0};
};

Aes::~Aes()
{
    if (_pimpl) {
        _pimpl.reset();
    }
}

Aes::Aes()
    : _pimpl(new Aes::pimpl)
{
}
```

```c++
AesEncrypt::AesEncrypt(Key256 const & k, Key128 const & iv)
    : Aes()
{
    if (_pimpl) {
      _pimpl->_cipher.reset(new CryptoPP::CTR_Mode<CryptoPP::AES>::Encryption);
      _pimpl->_cipher.get()->SetKeyWithIV(k.bytes(), k.length()/8, iv.bytes());
      _pimpl->_filter.reset(new CryptoPP::StreamTransformationFilter(*_pimpl->_cipher.get(), NULL, CryptoPP::BlockPaddingSchemeDef::NO_PADDING));
    }
}

int AesEncrypt::process(int inlen, sizebounded<unsigned char, Aes::datasz> & inoutbuf)
{
    if (! _pimpl->_filter) { return -1; }

    if (inlen > 0) {
      _pimpl->_filter.get()->Put(inoutbuf.ptr(), inlen); }

    if (!_pimpl->_filter.get()->AnyRetrievable()) {
      return 0; }

    size_t len = std::min(_pimpl->_filter.get()->MaxRetrievable(), (unsigned long)Aes::datasz);
    len = _pimpl->_filter.get()->Get((unsigned char*)inoutbuf.ptr(), len);

    return len;
}

int AesEncrypt::finish(int inpos, sizebounded<unsigned char, Aes::datasz> & outbuf)
{
    if (! _pimpl->_filter) { return -1; }
    _pimpl->_filter.get()->MessageEnd();

    if (!_pimpl->_filter.get()->AnyRetrievable()) {
      return 0; }

    size_t len = std::min(_pimpl->_filter.get()->MaxRetrievable(), (unsigned long)Aes::datasz - inpos);
    len = _pimpl->_filter.get()->Get((unsigned char*)outbuf.ptr() + inpos, len);

    return len;
}
```

```c++
AesDecrypt::AesDecrypt(Key256 const & k, Key128 const & iv)
    : Aes()
{
    if (_pimpl) {
      _pimpl->_cipher.reset(new CryptoPP::CTR_Mode<CryptoPP::AES>::Decryption);
      _pimpl->_cipher.get()->SetKeyWithIV(k.bytes(), k.length()/8, iv.bytes());
      _pimpl->_filter.reset(new CryptoPP::StreamTransformationFilter(*_pimpl->_cipher.get(), NULL, CryptoPP::BlockPaddingSchemeDef::NO_PADDING));
    }
}

int AesDecrypt::process(int inlen, sizebounded<unsigned char, Aes::datasz> & inoutbuf)
{
    if (! _pimpl->_filter) { return -1; }

    if (inlen > 0) {
      _pimpl->_filter.get()->Put(inoutbuf.ptr(), inlen); }

    if (!_pimpl->_filter.get()->AnyRetrievable()) {
      return 0; }

    size_t len = std::min(_pimpl->_filter.get()->MaxRetrievable(), (unsigned long)Aes::datasz);
    len = _pimpl->_filter.get()->Get((unsigned char*)inoutbuf.ptr(), len);

    return len;
}

int AesDecrypt::finish(int inpos, sizebounded<unsigned char, Aes::datasz> & outbuf)
{
    if (! _pimpl->_filter) { return -1; }
    _pimpl->_filter.get()->MessageEnd();

    if (!_pimpl->_filter.get()->AnyRetrievable()) {
      return 0; }

    size_t len = std::min(_pimpl->_filter.get()->MaxRetrievable(), (unsigned long)Aes::datasz - inpos);
    len = _pimpl->_filter.get()->Get((unsigned char*)outbuf.ptr() + inpos, len);

    return len;
}
#endif

```
