declared in [AppId](appid.hpp.md)

```cpp

const Key256 AppId::salt()
{
    Key256 k;
    k.fromHex("a7261fc15f4e515c024810aef0350c2a295e13057b81695f87fa03778ec57e1d");
    return k;
}


const std::string AppId::appid()
{
    return "d1e75ce87730af78f59618b75031e592a2c0530fea018420c515e4f51cf1627a";
}

```
