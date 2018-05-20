with import <nixpkgs> {};

stdenv.mkDerivation rec {
    name = "env";

    #src = ./.;

    # Customizable development requirements
    nativeBuildInputs = [
        fstar
        z3
        git
    ];

    buildInputs = [
    ];

}

