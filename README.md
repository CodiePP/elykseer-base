# elykseer-base

[![](https://www.elykseer.com/images/elykseer.png)](https://codiepp.github.io/elykseer-base/)

cryptographic data archive; written in F#; envisaged to stay another 10 years

read more [here ..](https://codiepp.github.io/elykseer-base/)


## hacking

the preferred way to inspect/hack/program eLyKseeR is via [nix-shell](https://nixos.org).


## compilation

### key for signing

before compilation, prepare a new RSA key for signing the assembly:

> ``sn -k eLyKseeR.snk``

(the linker will then expect the key in the file "eLyKseeR.snk")

### IDE support

> ``monodevelop eLyKseeR-base.Mono.sln &``

or open the [VisualStudio solution](eLyKseeR-base.Win32.sln) on Windows.


## references

[managed OpenSSL](https://github.com/openssl-net/openssl-net)

[sharpPRNG](https://github.com/CodiePP/prngsharp) install from [nuget](https://www.nuget.org/packages/sharpPRNG) or compile from submodule.

install in path ./packages:

> ``nuget install sharpPRNG``


## C++ native code

We are migrating the code to C++. Some external utilities are require to be installed in the directory 'ext/', see the README.

* [Gitalk](https://github.com/CodiePP/gitalk.git) to extract code from the literate source files.
* [SizeBounded](https://github.com/CodiePP/sizebounded.git) provides secure buffers bounded by their size

Preparation: add a symbolic link from cpp/src/ to cpp/src/lxr, so that the include files are found.

The code is extracted and assembled with the following script:

```
cd cpp
./mk_cpp.sh
```

Then, the CMakeLists.txt files will find the source and produce a Makefile which drives the compilation.

```
cd ..
mkdir BUILD
cd BUILD
cmake ..
make -j 4
```


## partners

[ ![](http://www.sbclab.com/img/sbclsml.png)](http://www.sbclab.com)

[ ![](http://www.icadia.ch/img/ICADIA_Shape_Text.png)](http://www.icadia.ch)

