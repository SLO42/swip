# Changelog

All notable changes to SWIP (Sam's Wonderful Impressive Card Park) will be documented in this file.

## [2.0.0] - 2026-03-02

### Added — Class System
- **3 mechanically distinct classes** via ClassesManagerReborn:
  - **SaucyEnchiladas** — Gas, area denial, sustain (NatureBrown)
  - **ASourFruit** — Bounces, explosions, ricochet chaos (FirepowerYellow)
  - **Synogence** — Fire, ice, health, size lock (DestructiveRed)
- 3 class entry cards (Common, all positive stats, no drawbacks)
- 22 gated cards per class + 30 unclassed cards = 99 total

### Added — New Cards (39)
**SaucyEnchiladas (12 new):**
- Corrosive Spray (Common), Noxious Fumes (Common)
- Second Wind (Uncommon), Miasma (Uncommon)
- Biohazard (Rare)
- Plague Doctor (Epic), Chemical Warfare (Epic), Life Drain (Epic), Contagion (Epic), Dead Zone (Epic)
- Pandemic (Legendary), Eternal Mist (Legendary)

**ASourFruit (9 new):**
- Chain Reaction (Uncommon)
- Ricochet Rush (Rare)
- Nuclear Option (Epic), Cluster Bomb (Epic), Seismic Impact (Epic), Volatile Payload (Epic), Aftershock (Epic)
- Singularity (Legendary), Perpetual Motion (Legendary)

**Synogence (9 new):**
- Frostbite (Uncommon), Core Stability (Uncommon)
- Thermal Shock (Epic), Cauterize (Epic), Permafrost (Epic), Glacial Armor (Epic), Inferno (Epic)
- Absolute Zero (Legendary), Supernova (Legendary)

**Unclassed (6 new):**
- Underdog (Epic), Momentum (Epic), Berserker (Epic)
- Extra Life (Legendary), Last Stand (Legendary), Nullifier (Legendary)

### Added — New Effects
- Freeze effect system (FreezeEffect + FreezeDamageEffect) — blue ice ring visual, velocity slow
- Stat-on-bounce effect (StatOnBounceEffect) — configurable damage/speed multipliers per bounce
- Size lock effect (SizeLockEffect) — prevents size growth above locked value

### Added — New Rarities
- Epic (purple, 8% spawn rate)
- Legendary (gold, 5% spawn rate)

### Changed — Identity Cleanup
- Replaced real names with class codenames: Daniel → ASourFruit, Max → Synogence, Sam → SaucyEnchiladas
- Renamed "One Punch Dan" → "One Punch"
- Updated all card descriptions referencing real names

### Changed — Card Rebalance
- Removed Extra Life (+1 respawn) from Special Sauce — now a standalone Legendary card with -10% speed trade-off
- All cards registered through CardRegistry for class system integration

### Dependencies
- Added ClassesManagerReborn (Root-Classes_Manager_Reborn-1.3.1)

## [1.1.0] - 2026-03-02

### Changed
- Gas effects (poison clouds, mists, chemicals) now use circular soft particles instead of square
- Gas particles have turbulence noise for swirling, organic movement
- Longer particle lifetimes and slower speeds for a lingering, ghastly feel
- Softer alpha falloff on gas particles for wispy dissolved edges

### Cards Affected
- Agent Orange, Acid Rain, Zyklon B, Gas Leak, Napalm (poison/damage clouds)
- Healing Mist (healing cloud)
- Smoke Bomb (poison aura)
- TrailBlazer (fire/poison trail zones)
- Come Here Bro (gravity aura uses zone visuals)

## [1.0.0] - 2026-03-02

### Added
- Initial release with 60 custom cards
- Three themed card sets: Daniel (ASourFruit), Max (Synogence), Sam (SaucyEnchiladas)
- Brotherhood/General card set
- Custom Mythic rarity (magenta, 2% spawn rate)
- Effects: poison clouds, burn DOT, lasers, homing missiles, orbital strikes, gravity auras, trails
