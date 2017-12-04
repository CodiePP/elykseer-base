declared in [Key](key.hpp.md)

allocate a buffer with given size
```cpp

/*
Key::Key(int bits)
{
    Random rng;
    _buffer.reset(new char[bits/8]);
    char *cptr = _buffer.get();
    for (int i=0; i<bits/8; i+=4) {
        uint32_t r = rng.random();
        *(cptr+i) = r & 0xff;
        *(cptr+i+1) = (r >> 8) & 0xff;
        *(cptr+i+2) = (r >> 16) & 0xff;
        *(cptr+i+3) = (r >> 24) & 0xff;
    }
} */

```