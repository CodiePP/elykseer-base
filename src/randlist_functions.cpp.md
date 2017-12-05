declared in [RandList](randlist.hpp.md)

```cpp

std::vector<int> mklist(int lo0, int hi0)
{
    int step = 1;   // the increment
    int lo = lo0;
    int hi = hi0;
    if (lo0 > hi0) {
        step = -1;
    }
    int n = abs(hi - lo) + 1;
    std::vector<int> res(n);
    int i = 0;
    int d = lo;
    while (i < n) {
        res[i++] = d;
        d += step;
    }
    return res;
}

std::vector<int> permutation(std::vector<int> vs)
{
    lxr::Random rng;
    int n = vs.size();
    for (int i=0; i<n; i++) {
        int r = rng.random(n);
        if (i == r) { r = n - i; }
        // swap i with r positions
        auto t = vs[i];
        vs[i] = vs[r];
        vs[r] = t;
    }
    return vs;
}

std::vector<int> RandList::Make(int lo, int hi)
{
    return permutation(mklist(lo, hi));
}


```