# SWIP — Sam's Wonderful Impressive Card Park

A custom card pack mod for [ROUNDS](https://store.steampowered.com/app/1557740/ROUNDS/) featuring 60 cards themed around three brothers and their chaotic arsenal.

## About

SWIP adds 60 cards across themed sets — each brother brings their own flavor of destruction, plus shared Brotherhood and Chaos Arsenal cards. Cards range from stat tweaks to full visual effects like poison clouds, laser beams, homing missiles, orbital strikes, and burn damage.

### Card Sets

| Set | Creator | Cards | Flavor |
|-----|---------|-------|--------|
| Daniel's Cards | ASourFruit | 9 | Sour, acidic, poison clouds |
| Max's Cards | Synogence | 8 | Precision tech, lasers, drones |
| Sam's Cards | SaucyEnchiladas | 8 | Spice, fire, smoke, napalm |
| Brotherhood | Shared | 26 | Chaos, utility, gravity, terrain |
| Chaos Arsenal | Expansion | 9 | Burn, holy light, orbital, mythic |

Includes a custom **Mythic** rarity (magenta, 2% spawn rate) for The Final Boss card.

### Effects

- **Gas clouds** — Swirling circular particles with turbulence noise (poison, healing, slow)
- **Burn damage** — Fire DOT with flickering ring visual
- **Laser beams** — Multi-layer line renderer with raycasted hits
- **Homing missiles** — Physics-based projectiles with steering
- **Orbital strikes** — Multi-explosion block-triggered attacks
- **Gravity aura** — Pulsing pull field affecting all players in radius
- **Fire/poison trails** — Zone spawning along projectile paths
- **Terrain destruction** — Break the map on hit

## Installation

Install via [r2modman](https://thunderstore.io/package/ebkr/r2modman/) or [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager).

### Dependencies

- [BepInEx Pack for ROUNDS](https://thunderstore.io/c/rounds/p/BepInEx/BepInExPack_ROUNDS/) 5.4.1901
- [UnboundLib](https://thunderstore.io/c/rounds/p/willis81808/UnboundLib/) 3.2.14
- [ModdingUtils](https://thunderstore.io/c/rounds/p/Pykess/ModdingUtils/) 0.4.8
- [RarityLib](https://thunderstore.io/c/rounds/p/Root/RarityLib/) 1.2.8

## Building from Source

Requires [.NET 6+ SDK](https://dotnet.microsoft.com/download) and game DLLs in `lib/` (not included — copy from your ROUNDS install).

```bash
# Build + package for Thunderstore
./build.sh        # Linux/Mac
./build.ps1       # Windows PowerShell

# Build only
dotnet build src/SWIP.csproj -c Release
```

Output: `SWIP-<version>.zip` ready for Thunderstore upload.

## Releasing

```bash
# Bump version, build, package
./scripts/release.sh patch    # or: major, minor, 1.2.3

# Tag and push (triggers GitHub Actions)
git tag v<version>
git push origin main --tags
```

The GitHub Actions workflow builds, packages, and creates a GitHub release automatically on version tags.

## Project Structure

```
src/
  Plugin.cs          # BepInEx entry point, card registration
  Cards/             # 60 card definitions (one per file)
  Effects/           # 17 effect components (particles, lasers, missiles, etc.)
thunderstore/
  manifest.json      # Thunderstore package metadata
  README.md          # Mod page description
  CHANGELOG.md       # Version history
  icon.png           # Mod icon (256x256)
scripts/
  release.sh         # Version bump + build + package helper
.github/workflows/
  release.yml        # Automated release on version tags
```

## Credits

Development assisted by [Claude Code](https://github.com/anthropics/claude-code) with [Claude Flow V3](https://github.com/ruvnet/claude-flow) orchestration by [rUv](https://github.com/ruvnet).

## License

All rights reserved.
