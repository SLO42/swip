#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$SCRIPT_DIR"

VERSION=$(jq -r '.version_number' thunderstore/manifest.json)
echo "Building SWIP v${VERSION}..."
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
[ -f thunderstore/CHANGELOG.md ] && cp thunderstore/CHANGELOG.md build/

cd build
zip -r "../SWIP-${VERSION}.zip" .
cd ..
rm -rf build

echo "Done! Package: SWIP-${VERSION}.zip"
