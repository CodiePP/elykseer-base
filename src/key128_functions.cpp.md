declared in [Key128](key128.hpp.md)

```cpp

void Key128::map(std::function<void(const int, const char)> f) const
{
    int i = 0;
    for (auto const c : _buffer) {
        f(i, c);
        i++;
    }
}

void Key128::transform(std::function<char(const int, const char)> f)
{
    for (int i=0; i < length() / 8; i++) {
        _buffer[i] = f(i, _buffer[i]);
    }
}


```