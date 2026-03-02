#!/usr/bin/env bash
set -euo pipefail

# release.sh — Bump version, update manifest, build, package, and tag for release.
#
# Usage:
#   ./scripts/release.sh <major|minor|patch>   Auto-bump version
#   ./scripts/release.sh 1.2.3                 Set explicit version

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
ROOT="$SCRIPT_DIR/.."
cd "$ROOT"

MANIFEST="thunderstore/manifest.json"
CHANGELOG="thunderstore/CHANGELOG.md"

# --- Parse current version ---
CURRENT=$(jq -r '.version_number' "$MANIFEST")
IFS='.' read -r MAJOR MINOR PATCH <<< "$CURRENT"

# --- Determine new version ---
case "${1:-}" in
  major) MAJOR=$((MAJOR + 1)); MINOR=0; PATCH=0 ;;
  minor) MINOR=$((MINOR + 1)); PATCH=0 ;;
  patch) PATCH=$((PATCH + 1)) ;;
  [0-9]*.[0-9]*.[0-9]*) IFS='.' read -r MAJOR MINOR PATCH <<< "$1" ;;
  *)
    echo "Usage: $0 <major|minor|patch|X.Y.Z>"
    echo "  Current version: $CURRENT"
    exit 1
    ;;
esac

NEW_VERSION="${MAJOR}.${MINOR}.${PATCH}"
echo "Version: $CURRENT -> $NEW_VERSION"

# --- Update manifest.json ---
jq --arg v "$NEW_VERSION" '.version_number = $v' "$MANIFEST" > "$MANIFEST.tmp"
mv "$MANIFEST.tmp" "$MANIFEST"
echo "Updated $MANIFEST"

# --- Add changelog placeholder if new version section doesn't exist ---
if ! grep -q "## \[$NEW_VERSION\]" "$CHANGELOG" 2>/dev/null; then
  TODAY=$(date +%Y-%m-%d)
  # Insert new section after the first "# Changelog" header line
  HEADER="## [$NEW_VERSION] - $TODAY"
  sed -i "0,/^## \[/{s|^## \[|${HEADER}\n\n### Changed\n- \n\n## [|}" "$CHANGELOG"
  echo "Added changelog section for $NEW_VERSION — edit $CHANGELOG before committing"
fi

# --- Build ---
echo ""
echo "Building..."
dotnet build src/SWIP.csproj -c Release
DLL="src/bin/Release/netstandard2.1/SWIP.dll"
if [ ! -f "$DLL" ]; then
  echo "ERROR: Build failed — SWIP.dll not found"
  exit 1
fi

# --- Copy built DLL into thunderstore/ for CI ---
cp "$DLL" thunderstore/SWIP.dll
echo "Copied SWIP.dll to thunderstore/"

echo ""
echo "Next steps:"
echo "  1. Review and edit $CHANGELOG with your changes"
echo "  2. git add -A && git commit -m \"release: v${NEW_VERSION}\""
echo "  3. Push / merge to main — CI will package and create the GitHub release"
