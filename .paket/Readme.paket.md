## To create packages:
1. Build in release
2. PM> `.paket/paket.bootstrapper.exe` only needed for downloading paket.exe
3. PM> `.paket/paket.exe pack output .\publish symbols`
4. Packages are in the publish folder.

Docs: https://fsprojects.github.io/Paket/getting-started.html