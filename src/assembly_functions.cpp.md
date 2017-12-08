declared in [Assembly](assembly.hpp.md)

```cpp

template <int n>
bool Assembly<n>::encrypt(Key256 const & k, Key256 & iv)
{
  return false;
}

template <int n>
bool Assembly<n>::decrypt(Key256 const & k)
{
  return false;
}

template <int n>
Key256 Assembly<n>::mkChunkId(int idx) const
{
  return Key256();
}

template <int n>
bool Assembly<n>::extractChunks() const
{
  return false;
}

template <int n>
bool Assembly<n>::extractChunk(int idx) const
{
  return false;
}

template <int n>
bool Assembly<n>::insertChunks()
{
  return false;
}

template <int n>
int Assembly<n>::free() const
{
  return 0;
}

template <int n>
bool Assembly<n>::isWritable() const
{
  return false;
}

template <int n>
bool Assembly<n>::isEncrypted() const
{
  return false;
}

template <int n>
int Assembly<n>::addData(const sizebounded<char, datasz()> & d)
{
  return 0;
}

template <int n>
int Assembly<n>::getData(int pos0, int pos1, sizebounded<char, datasz()> & d) const
{
  return 0;
}


```
