declared in [Random](random.hpp.md)

```cpp

uint32_t Random::random() const
{
    return _rng->get_uint32();
}

uint32_t Random::random(int max) const
{
    uint32_t r0 = _rng->get_uint32();
    double r = double(r0) * double(max) / (pow(2, 32) - 1) - 0.5;
    long int r1 = lrint(r);
    return (uint32_t)r1;
}

```