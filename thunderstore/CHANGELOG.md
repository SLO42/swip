# Changelog

All notable changes to SWIP (Sam's Wonderful Impressive Card Park) will be documented in this file.

## [2.2.0] - 2026-03-02

### Fixed — Playtest Issues (#9–#17)
- **#9/#15 Drone Swarm missiles** — missiles no longer chain-explode each other; explosions skip sibling missiles and sky projectiles
- **#11 Brotherly Love** — dangerous cards (Mom Said My Turn, Brotherly Love) replaced with inert versions during deck swaps to prevent re-triggering
- **#13 Come Here Bro** — gravity pull now uses direct position modification (strength 8) to bypass ROUNDS' PlayerVelocity override
- **#14 Wild Card** — simplified to native `gun.numberOfProjectiles + gun.spread` instead of broken Instantiate cloning
- **#17 Synapse Fire** — custom WaveMotionEffect applies sine-wave oscillation via position offset, preserving bullet direction

### Changed — Card Reworks
- **Smite** — reworked from explosion-on-hit to holy golden beam from top of screen; penetrates everything (players, objects, terrain), deals damage along entire path
- **Satellite Uplink** — reworked from orbital explosion to red laser on block at owner's position; penetrates destructables, stops on solid terrain; acts as defensive shield from above
- **Scorched Sky** — reworked from orbital-strike-on-block to rain-of-fire on bullet hit; spawns 5 projectiles from top of screen, inherits cloud/gas effects from owner, bounces scale with gun reflects
- **Mom Said My Turn** — self-replaces with "Yay It's My Turn!" after granting random cards

### Improved — Explosion System (SWIPExplosion)
- Vanilla-style visuals: white-hot flash, expanding ring, flying spark particles with gravity, lingering grey smoke puffs
- Explosions now damage destructables (boxes, terrain pieces) via Damagable.CallTakeDamage
- Knockback uses Collider2D.attachedRigidbody for reliable physics object detection
- Color presets: Fire (orange), Orbital (blue), Missile (red)

### Improved — Missiles
- Missiles spawn once per firing action (frame debounce), not once per bullet
- Grace period only blocks owner's bullets — missiles still hit terrain and enemies on spawn
- Missile thruster exhaust trail (TrailRenderer, orange fade)

### Added — New Effects
- SmiteBeamEffect — holy beam visual with golden core + glow
- SatelliteLaserEffect — red laser visual with quick flash
- ScorchedSkyEffect — falling sky projectiles with streak + trail visuals
- WaveMotionEffect — sine-wave bullet oscillation
- SWIPExplosion smoke particles

### Added — New Cards
- Yay It's My Turn! (inert replacement for Mom Said My Turn)
- Sharing Is Caring (inert replacement for Brotherly Love during swaps)
- Orbital Test (debug card: beam on hit + laser on block)

### Removed
- SideshootEffect (replaced by native gun stats for Wild Card)
- Old SmiteOnHitEffect / SmiteCoroutineRunner (replaced by SmiteBeamEffect)
- Old OrbitalStrikeEffect (replaced by SatelliteLaserEffect)

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
