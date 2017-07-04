# elykseer-base

cryptographic data archive; written in F#; envisaged to stay another 10 years

read more [here ..](https://codiepp.github.io/elykseer-base/)



## compilation

### key for signing

before compilation, prepare a new RSA key for signing the assembly:

> ``sn -k fLXR.snk``

(the linker will then expect the key in the file "fLXR.snk")

### IDE support

> ``monodevelop fLXR.sln &``

or open the solution in VisualStudio.


## references

[managed OpenSSL](https://github.com/openssl-net/openssl-net)

[sharpPRNG](https://github.com/CodiePP/prngsharp) install from [nuget](https://www.nuget.org/packages/sharpPRNG) or compile from submodule.

install in path ./packages:

> ``nuget install sharpPRNG``


