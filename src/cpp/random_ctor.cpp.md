declared in [Random](random.hpp.md)

```cpp

Random::Random()
{
    _rng.reset(new prngCpp::MT19937);
}

```