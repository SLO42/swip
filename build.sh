#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$SCRIPT_DIR"

echo "Building SWIP..."
dotnet build src/SWIP.csproj -c Release

DLL="src/bin/Release/netstandard2.1/SWIP.dll"
if [ ! -f "$DLL" ]; then
    echo "ERROR: Build failed - SWIP.dll not found"
    exit 1
fi

echo "Packaging for Thunderstore..."
mkdir -p build
cp "$DLL" build/SWIP.dll
cp thunderstore/manifest.json build/
cp thunderstore/README.md build/
cp thunderstore/icon.png build/

cd build
zip -r ../SWIP-1.0.0.zip manifest.json README.md icon.png SWIP.dll
cd ..
rm -rf build

echo "Done! Package: SWIP-1.0.0.zip"
