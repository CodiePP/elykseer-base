with import <nixpkgs> {};

stdenv.mkDerivation rec {
    name = "env";

    #src = ./.;

    # Customizable development requirements
    nativeBuildInputs = [
        cmake
        git
    ];

    buildInputs = [
        openssl
        zlib
        boost
    ];

}

