declared in [Aes](aes.hpp.md)

```c++
struct Aes::pimpl {
    pimpl() {};
    ~pimpl() { if (_ctx) { EVP_CIPHER_CTX_free(_ctx); _ctx = NULL; } };
    EVP_CIPHER_CTX *_ctx;
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
    EVP_add_cipher(EVP_aes_256_cbc());
    _pimpl->_ctx = EVP_CIPHER_CTX_new();
}
```

```c++
AesEncrypt::AesEncrypt(Key256 const & k, Key128 const & iv)
    : Aes()
{
    if (_pimpl->_ctx && 
        EVP_EncryptInit_ex(_pimpl->_ctx, EVP_aes_256_cbc(), NULL, (const unsigned char *)k.bytes(), (const unsigned char *)iv.bytes()) != 1)
    {
        std::clog << "failed to init encryption!" << std::endl;
        EVP_CIPHER_CTX_free(_pimpl->_ctx);
        _pimpl->_ctx = NULL;        
    }
}

int AesEncrypt::process(int inlen, sizebounded<unsigned char, 1024> & inoutbuf)
{
    if (! _pimpl->_ctx) { return -1; }
    int len = 0;
    unsigned char mbuf[1024];
    inoutbuf.map([&inlen,&mbuf](const int i, const char c)->void {
        if (i < inlen) { mbuf[i] = c; }
        else { mbuf[i] = '\0'; }
    });
    unsigned char tbuf[1024];
    try {
        if (EVP_EncryptUpdate(_pimpl->_ctx, tbuf, &len, mbuf, inlen) != 1) {
            throw "AesDecrypt::process failed";
        }
        inoutbuf.transform([&len,&tbuf](const int i, const char c)->char {
            if (i < len) { return tbuf[i]; }
            else { return '\0'; }
        });
    } catch (...) {}
    return len;
}

int AesEncrypt::finish(sizebounded<unsigned char, 1024> & outbuf)
{
    if (! _pimpl->_ctx) { return -1; }
    int len = 0;
    unsigned char tbuf[1024];
    // if (EVP_EncryptFinal_ex(_pimpl->_ctx, tbuf, &len) != 1) {
    //     len = -1;
    // }
    outbuf.transform([&len,&tbuf](const int i, const char c)->char {
        if (i < len) { return tbuf[i]; }
        else { return '\0'; }
    });
    // EVP_CIPHER_CTX_free(_pimpl->_ctx);
    // _pimpl->_ctx = NULL;
    return len;
}
```

```c++
AesDecrypt::AesDecrypt(Key256 const & k, Key128 const & iv)
    : Aes()
{
    if (_pimpl->_ctx && 
        EVP_DecryptInit_ex(_pimpl->_ctx, EVP_aes_256_cbc(), NULL, (const unsigned char *)k.bytes(), (const unsigned char *)iv.bytes()) != 1)
    {
        EVP_CIPHER_CTX_free(_pimpl->_ctx);
        _pimpl->_ctx = NULL;
    }
}

int AesDecrypt::process(int inlen, sizebounded<unsigned char, 1024> & outbuf)
{
    if (! _pimpl->_ctx) { return -1; }
    int len = 0;
    unsigned char tbuf[1024];
    if (EVP_DecryptUpdate(_pimpl->_ctx, tbuf, &len, outbuf.ptr(), inlen) != 1) {
        throw "AesDecrypt::process failed";
    }
    return len;
}

int AesDecrypt::finish(sizebounded<unsigned char, 1024> & outbuf)
{
    if (! _pimpl->_ctx) { return -1; }
    int len = 0;
    if (EVP_DecryptFinal_ex(_pimpl->_ctx, (unsigned char*)outbuf.ptr(), &len) != 1) {
        len = -1;
    }
    EVP_CIPHER_CTX_free(_pimpl->_ctx);
    _pimpl->_ctx = NULL;
    return len;
}
```
