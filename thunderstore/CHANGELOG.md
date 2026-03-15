# Changelog

All notable changes to SWIP (Sam's Wonderful Impressive Card Park) will be documented in this file.

## [2.3.2] - 2026-03-15

### Fixed
- **#23 Scorched Earth deleting players** — added root-hierarchy player check to TerrainBreakerEffect so direct hits on players no longer destroy the player object

## [2.3.1] - 2026-03-15

### Removed
- **Carpet Bomb** card and registration
- **Drone Swarm** card and registration
- **Scorched Sky** card, registration, and ScorchedSkyEffect
- **MissileBarrageEffect** — missile homing/spawning effect
- Missile/sky projectile chain-explosion guards from SWIPExplosion (no longer needed)

## [2.3.0] - 2026-03-03

### Added — Silly/Chaos Card Pack (~50 new unclassed cards)

**Radial Shot & Bullet Adders (4):**
- Radial Spread (Uncommon), Bullet Storm (Uncommon), Bullet Hail (Rare), Bullet Flood (Epic)

**Clip Dump (3):**
- Clip Dump (Uncommon), Clip Purge (Rare), Clip Apocalypse (Epic)

**Slow Cards (3):**
- Slow Drip (Common), Slow Pour (Uncommon), Slow Tide (Rare)

**Bounce Count Cards (6):**
- Bounce x2 (Common), Bounce x5 (Common), Bounce x10 (Uncommon), Bounce x25 (Rare), Bounce x50 (Epic), Bounce x100 (Legendary)
- All include screen-edge bouncing via ScreenBounceEffect

**Bounce Modifier Cards (7):**
- Ricochet Roulette (Uncommon), Featherfall (Uncommon), Shockwave Bounce (Uncommon), Ice Ricochet (Uncommon)
- Chaos Ricochet (Rare), Vortex Bounce (Rare), Scorching Bounce (Rare)

**Stat Boost Cards (3):**
- Wide Spread (Common), Bullet Speed+ (Common), Rapid Fire (Common)

**Ammo/Reload Cards (2):**
- Infinite Ammo (Legendary), Quick Draw (Rare)

**Unique Mechanic Cards (8):**
- Bullet Warp (Epic) — teleport to bullet hit location
- Snake Rain (Rare) — homing snakes spawn when hit
- Speed Demon (Uncommon) — swap movespeed for damage
- Chaos Stats (Rare) — randomize 2 stat pairs
- Rising Tide (Rare) — boost above-baseline stats +20%
- Double Down (Uncommon) — 50/50 double or halve weak stats
- All In (Epic) — randomize all stats +/-50%
- Effect Amplifier (Epic) — boost all effect sizes x1.5

**Card Manipulation Cards (8):**
- New Hand (Epic) — replace own hand with random cards
- All-In-One (Legendary) — all cards become copies of 1 random
- Card Thief (Rare) — force others to swap random cards
- Shuffle & Deal (Epic) — others shuffle and redraw
- Card Tornado (Legendary) — all players' cards pooled and redistributed
- High Stakes (Rare) — gamble to steal/lose stats vs others
- Stat Leech (Epic) — siphon better stats from other players
- Copycat (Legendary) — clone any card for entire hand

**Custom Projectile Cards (2):**
- Angry Birds (Epic) — homing bird projectiles with swooping arcs
- Squid Ink (Epic) — homing squids with sinusoidal swim + ink cloud slow

**On-Block/On-Hit Spawner Cards (3):**
- Nacho Block (Uncommon) — spawn nachos on block (heal allies, damage enemies)
- Lemon Drop (Rare) — blinding lemon zones on hit/bounce
- Tortilla Shield (Rare) — invulnerable wrap on block (3s, immobile)

### Added — New Effects (18)
- RadialShotEffect — distributes bullets at equal angles (360/count)
- ScreenBounceEffect — bounces bullets off camera viewport edges
- ClipDumpEffect — forces rapid-fire entire clip on trigger
- SlowOnHitEffect — applies slow debuff on hit (mint green visual)
- FeatherfallBounceEffect — reduces gravity per bounce
- ChaosRicochetEffect — swaps damage/speed on bounce
- VortexBounceEffect — pulls enemies toward bounce point
- ShockwaveBounceEffect — pushes enemies away from bounce point
- IceRicochetEffect — slows enemies near bounce point
- ScorchingBounceEffect — burns near bounce (escalates per bounce)
- BounceRandomSizeEffect — random size change + speed up on bounce
- TeleportToBulletEffect — teleports owner to bullet hit location
- SnakeRainEffect + SnakeHomingBehaviour — wavy homing snakes
- HomingBirdEffect + BirdHomingBehaviour — swooping homing birds
- HomingSquidEffect + SquidHomingBehaviour — swimming squids with ink clouds
- NachosOnBlockEffect + NachoProjectile — radial nacho burst on block
- TortillaWrapEffect — invulnerable carb shell on block
- LemonDropEffect — blinding lemon zones on hit/bounce

### Added — Inert Placeholders (8)
- New Hand Used, All-In-One Used, Card Thief Used, Shuffle & Deal Used, Card Tornado Used, High Stakes Used, Stat Leech Used, Copycat Used

### Changed
- BrotherlyLove dangerous cards set expanded to include all new manipulation cards
- BrotherlyLove dangerousCards/replacements made public static for extensibility

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
