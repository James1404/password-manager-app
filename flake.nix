{
  description = "Shopping site";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, flake-utils, nixpkgs, ... } @ inputs:
    flake-utils.lib.eachDefaultSystem
      (system:
        let
          pkgs = import nixpkgs { inherit system; };
        in {
          devShells.default = (pkgs.mkShell rec {
            packages = with pkgs; [
              # Dotnet dev stuff
              dotnet-sdk_9
              csharp-ls
              pkg-config

              # Packages
              avalonia
              sqlite

              xorg.libICE
              xorg.libSM
              xorg.libX11
              xorg.libXcursor
              xorg.libXext
              xorg.libXi
              xorg.libXrandr
              liberation_ttf
              fontconfig
              libgdiplus
              skia
            ];

            LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath packages;
          });
        });
}
