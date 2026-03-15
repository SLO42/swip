using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Utils.UI;
using UnityEngine;
using SWIP.Cards;

namespace SWIP
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.rarity.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModId = "com.sam.rounds.swip";
        private const string ModName = "SWIP";
        private const string Version = "2.0.0";

        public static bool UseClasses;
        private static ConfigEntry<bool> cfgUseClasses;
        private static bool cmrPresent;

        void Awake()
        {
            new Harmony(ModId).PatchAll();

            // Register custom rarities
            RarityLib.Utils.RarityUtils.AddRarity("Epic", 0.08f, new Color(0.6f, 0.2f, 0.8f), new Color(0.4f, 0.1f, 0.5f));
            RarityLib.Utils.RarityUtils.AddRarity("Legendary", 0.05f, new Color(1f, 0.84f, 0f), new Color(0.8f, 0.6f, 0f));
            RarityLib.Utils.RarityUtils.AddRarity("Mythic", 0.02f, new Color(1f, 0.2f, 0.8f), new Color(0.6f, 0.1f, 0.5f));

            cmrPresent = Chainloader.PluginInfos.ContainsKey("root.classes.manager.reborn");

            cfgUseClasses = Config.Bind(
                "General",
                "UseClasses",
                cmrPresent,
                "Enable class gating (requires ClassesManagerReborn). When false, all cards are a flat pool."
            );

            if (cfgUseClasses.Value && !cmrPresent)
            {
                Logger.LogWarning("[SWIP] UseClasses enabled but ClassesManagerReborn not installed. Forcing to false.");
                cfgUseClasses.Value = false;
            }

            UseClasses = cfgUseClasses.Value;
            Logger.LogInfo($"[SWIP] UseClasses = {UseClasses} (CMR present: {cmrPresent})");
        }

        void Start()
        {
            Unbound.RegisterClientSideMod(ModId);
            Unbound.RegisterMenu(ModName, () => { }, menu =>
            {
                MenuHandler.CreateToggle(UseClasses, "Use Classes (requires restart)", menu, value =>
                {
                    if (value && !cmrPresent)
                    {
                        Logger.LogWarning("[SWIP] ClassesManagerReborn not installed — can't enable classes.");
                        return;
                    }
                    UseClasses = value;
                    cfgUseClasses.Value = value;
                });
            }, null, false);

            // === Entry Cards (register first) ===
            CustomCard.BuildCard<SaucyEnchiladasEntry>(ci => CardRegistry.Register("SaucyEnchiladas Entry", ci));
            CustomCard.BuildCard<ASourFruitEntry>(ci => CardRegistry.Register("ASourFruit Entry", ci));
            CustomCard.BuildCard<SynogenceEntry>(ci => CardRegistry.Register("Synogence Entry", ci));

            // === SaucyEnchiladas Class Cards ===
            CustomCard.BuildCard<SmokeBomb>(ci => CardRegistry.Register("Smoke Bomb", ci));
            CustomCard.BuildCard<SalsaVerde>(ci => CardRegistry.Register("Salsa Verde", ci));
            CustomCard.BuildCard<CorrosiveSpray>(ci => CardRegistry.Register("Corrosive Spray", ci));
            CustomCard.BuildCard<NoxiousFumes>(ci => CardRegistry.Register("Noxious Fumes", ci));
            CustomCard.BuildCard<AcidRain>(ci => CardRegistry.Register("Acid Rain", ci));
            CustomCard.BuildCard<GasLeak>(ci => CardRegistry.Register("Gas Leak", ci));
            CustomCard.BuildCard<ToxicRelationship>(ci => CardRegistry.Register("Toxic Relationship", ci));
            CustomCard.BuildCard<HealingMist>(ci => CardRegistry.Register("Healing Mist", ci));
            CustomCard.BuildCard<ExtraSpicy>(ci => CardRegistry.Register("Extra Spicy", ci));
            CustomCard.BuildCard<SecondWind>(ci => CardRegistry.Register("Second Wind", ci));
            CustomCard.BuildCard<Miasma>(ci => CardRegistry.Register("Miasma", ci));
            CustomCard.BuildCard<SpecialSauce>(ci => CardRegistry.Register("Special Sauce", ci));
            CustomCard.BuildCard<EnchiladaTreatment>(ci => CardRegistry.Register("The Enchilada Treatment", ci));
            CustomCard.BuildCard<HotOnesChallenge>(ci => CardRegistry.Register("Hot Ones Challenge", ci));
            CustomCard.BuildCard<Biohazard>(ci => CardRegistry.Register("Biohazard", ci));
            CustomCard.BuildCard<PlagueDoctor>(ci => CardRegistry.Register("Plague Doctor", ci));
            CustomCard.BuildCard<ChemicalWarfare>(ci => CardRegistry.Register("Chemical Warfare", ci));
            CustomCard.BuildCard<LifeDrain>(ci => CardRegistry.Register("Life Drain", ci));
            CustomCard.BuildCard<Contagion>(ci => CardRegistry.Register("Contagion", ci));
            CustomCard.BuildCard<DeadZone>(ci => CardRegistry.Register("Dead Zone", ci));
            CustomCard.BuildCard<Pandemic>(ci => CardRegistry.Register("Pandemic", ci));
            CustomCard.BuildCard<EternalMist>(ci => CardRegistry.Register("Eternal Mist", ci));

            // === ASourFruit Class Cards ===
            CustomCard.BuildCard<RubberBullets>(ci => CardRegistry.Register("Rubber Bullets", ci));
            CustomCard.BuildCard<SourPunch>(ci => CardRegistry.Register("Sour Punch", ci));
            CustomCard.BuildCard<FruitSalad>(ci => CardRegistry.Register("Fruit Salad", ci));
            CustomCard.BuildCard<SourPatch>(ci => CardRegistry.Register("Sour Patch", ci));
            CustomCard.BuildCard<CitricAcid>(ci => CardRegistry.Register("Citric Acid", ci));
            CustomCard.BuildCard<OnePunchDan>(ci => CardRegistry.Register("One Punch", ci));
            CustomCard.BuildCard<Pinball>(ci => CardRegistry.Register("Pinball", ci));
            CustomCard.BuildCard<NoU>(ci => CardRegistry.Register("No U", ci));
            CustomCard.BuildCard<SnowballEffectCard>(ci => CardRegistry.Register("Snowball Effect", ci));
            CustomCard.BuildCard<AgentOrange>(ci => CardRegistry.Register("Agent Orange", ci));
            CustomCard.BuildCard<ZyklonB>(ci => CardRegistry.Register("Zyklon B", ci));
            CustomCard.BuildCard<ChainReaction>(ci => CardRegistry.Register("Chain Reaction", ci));
            CustomCard.BuildCard<BounceHouse>(ci => CardRegistry.Register("Bounce House", ci));
            CustomCard.BuildCard<ScorchedEarth>(ci => CardRegistry.Register("Scorched Earth", ci));
            CustomCard.BuildCard<RicochetRush>(ci => CardRegistry.Register("Ricochet Rush", ci));
            CustomCard.BuildCard<NuclearOption>(ci => CardRegistry.Register("Nuclear Option", ci));
            CustomCard.BuildCard<ClusterBomb>(ci => CardRegistry.Register("Cluster Bomb", ci));
            CustomCard.BuildCard<SeismicImpact>(ci => CardRegistry.Register("Seismic Impact", ci));
            CustomCard.BuildCard<VolatilePayload>(ci => CardRegistry.Register("Volatile Payload", ci));
            CustomCard.BuildCard<Aftershock>(ci => CardRegistry.Register("Aftershock", ci));
            CustomCard.BuildCard<Singularity>(ci => CardRegistry.Register("Singularity", ci));
            CustomCard.BuildCard<PerpetualMotion>(ci => CardRegistry.Register("Perpetual Motion", ci));

            // === Synogence Class Cards ===
            CustomCard.BuildCard<Arsonist>(ci => CardRegistry.Register("Arsonist", ci));
            CustomCard.BuildCard<LemonZest>(ci => CardRegistry.Register("Lemon Zest", ci));
            CustomCard.BuildCard<TrailBlazer>(ci => CardRegistry.Register("Trail Blazer", ci));
            CustomCard.BuildCard<TheAlgorithm>(ci => CardRegistry.Register("The Algorithm", ci));
            CustomCard.BuildCard<SynapseFire>(ci => CardRegistry.Register("Synapse Fire", ci));
            CustomCard.BuildCard<Firewall>(ci => CardRegistry.Register("Firewall", ci));
            CustomCard.BuildCard<GhostPepper>(ci => CardRegistry.Register("Ghost Pepper", ci));
            CustomCard.BuildCard<ProtocolOverride>(ci => CardRegistry.Register("Protocol Override", ci));
            CustomCard.BuildCard<BufferOverflow>(ci => CardRegistry.Register("Buffer Overflow", ci));
            CustomCard.BuildCard<RecursiveLoop>(ci => CardRegistry.Register("Recursive Loop", ci));
            CustomCard.BuildCard<HolyLight>(ci => CardRegistry.Register("Holy Light", ci));
            CustomCard.BuildCard<Frostbite>(ci => CardRegistry.Register("Frostbite", ci));
            CustomCard.BuildCard<CoreStability>(ci => CardRegistry.Register("Core Stability", ci));
            CustomCard.BuildCard<LaserPrecision>(ci => CardRegistry.Register("Laser Precision", ci));
            CustomCard.BuildCard<Napalm>(ci => CardRegistry.Register("Napalm", ci));
            CustomCard.BuildCard<ThermalShock>(ci => CardRegistry.Register("Thermal Shock", ci));
            CustomCard.BuildCard<Cauterize>(ci => CardRegistry.Register("Cauterize", ci));
            CustomCard.BuildCard<Permafrost>(ci => CardRegistry.Register("Permafrost", ci));
            CustomCard.BuildCard<GlacialArmor>(ci => CardRegistry.Register("Glacial Armor", ci));
            CustomCard.BuildCard<Inferno>(ci => CardRegistry.Register("Inferno", ci));
            CustomCard.BuildCard<AbsoluteZero>(ci => CardRegistry.Register("Absolute Zero", ci));
            CustomCard.BuildCard<Supernova>(ci => CardRegistry.Register("Supernova", ci));

            // === Unclassed Cards ===
            CustomCard.BuildCard<WelcomeCard>(ci => CardRegistry.Register("Welcome to the Park", ci));
            CustomCard.BuildCard<ControllerThrow>(ci => CardRegistry.Register("Controller Throw", ci));
            CustomCard.BuildCard<LilBroEnergy>(ci => CardRegistry.Register("Lil Bro Energy", ci));
            CustomCard.BuildCard<BigBroEnergy>(ci => CardRegistry.Register("Big Bro Energy", ci));
            CustomCard.BuildCard<VitaminC>(ci => CardRegistry.Register("Vitamin C", ci));
            CustomCard.BuildCard<Adrenaline>(ci => CardRegistry.Register("Adrenaline", ci));
            CustomCard.BuildCard<SugarRush>(ci => CardRegistry.Register("Sugar Rush", ci));
            CustomCard.BuildCard<RageQuit>(ci => CardRegistry.Register("Rage Quit", ci));
            CustomCard.BuildCard<SnackBreak>(ci => CardRegistry.Register("Snack Break", ci));
            CustomCard.BuildCard<AmmoHoarder>(ci => CardRegistry.Register("Ammo Hoarder", ci));
            CustomCard.BuildCard<ThickSkin>(ci => CardRegistry.Register("Thick Skin", ci));
            CustomCard.BuildCard<GlassCannon>(ci => CardRegistry.Register("Glass Cannon", ci));
            CustomCard.BuildCard<CouchCushionFort>(ci => CardRegistry.Register("Couch Cushion Fort", ci));
            CustomCard.BuildCard<WildCard>(ci => CardRegistry.Register("Wild Card", ci));
            CustomCard.BuildCard<Smite>(ci => CardRegistry.Register("Smite", ci));
            CustomCard.BuildCard<ComeHereBro>(ci => CardRegistry.Register("Come Here Bro", ci));
            CustomCard.BuildCard<MomSaidMyTurn>(ci => CardRegistry.Register("Mom Said My Turn", ci));
            CustomCard.BuildCard<BrotherlyLove>(ci => CardRegistry.Register("Brotherly Love", ci));
            CustomCard.BuildCard<DivineJudgment>(ci => CardRegistry.Register("Divine Judgment", ci));
            CustomCard.BuildCard<SatelliteUplink>(ci => CardRegistry.Register("Satellite Uplink", ci));
            CustomCard.BuildCard<Underdog>(ci => CardRegistry.Register("Underdog", ci));
            CustomCard.BuildCard<MomentumCard>(ci => CardRegistry.Register("Momentum", ci));
            CustomCard.BuildCard<Berserker>(ci => CardRegistry.Register("Berserker", ci));
            CustomCard.BuildCard<ExtraLife>(ci => CardRegistry.Register("Extra Life", ci));
            CustomCard.BuildCard<LastStand>(ci => CardRegistry.Register("Last Stand", ci));
            CustomCard.BuildCard<Nullifier>(ci => CardRegistry.Register("Nullifier", ci));
            CustomCard.BuildCard<TheFinalBoss>(ci => CardRegistry.Register("The Final Boss", ci));

            // === Radial Shot + Bullet Adders ===
            CustomCard.BuildCard<RadialSpread>(ci => CardRegistry.Register("Radial Spread", ci));
            CustomCard.BuildCard<BulletStorm>(ci => CardRegistry.Register("Bullet Storm", ci));
            CustomCard.BuildCard<BulletHail>(ci => CardRegistry.Register("Bullet Hail", ci));
            CustomCard.BuildCard<BulletFlood>(ci => CardRegistry.Register("Bullet Flood", ci));

            // === Clip Dump ===
            CustomCard.BuildCard<ClipDump>(ci => CardRegistry.Register("Clip Dump", ci));
            CustomCard.BuildCard<ClipPurge>(ci => CardRegistry.Register("Clip Purge", ci));
            CustomCard.BuildCard<ClipApocalypse>(ci => CardRegistry.Register("Clip Apocalypse", ci));

            // === Slow Cards ===
            CustomCard.BuildCard<SlowDrip>(ci => CardRegistry.Register("Slow Drip", ci));
            CustomCard.BuildCard<SlowPour>(ci => CardRegistry.Register("Slow Pour", ci));
            CustomCard.BuildCard<SlowTide>(ci => CardRegistry.Register("Slow Tide", ci));

            // === Bounce Count Cards ===
            CustomCard.BuildCard<BounceX2>(ci => CardRegistry.Register("Bounce x2", ci));
            CustomCard.BuildCard<BounceX5>(ci => CardRegistry.Register("Bounce x5", ci));
            CustomCard.BuildCard<BounceX10>(ci => CardRegistry.Register("Bounce x10", ci));
            CustomCard.BuildCard<BounceX25>(ci => CardRegistry.Register("Bounce x25", ci));
            CustomCard.BuildCard<BounceX50>(ci => CardRegistry.Register("Bounce x50", ci));
            CustomCard.BuildCard<BounceX100>(ci => CardRegistry.Register("Bounce x100", ci));

            // === Bounce Modifier Cards ===
            CustomCard.BuildCard<RicochetRoulette>(ci => CardRegistry.Register("Ricochet Roulette", ci));
            CustomCard.BuildCard<Featherfall>(ci => CardRegistry.Register("Featherfall", ci));
            CustomCard.BuildCard<ChaosRicochet>(ci => CardRegistry.Register("Chaos Ricochet", ci));
            CustomCard.BuildCard<VortexBounce>(ci => CardRegistry.Register("Vortex Bounce", ci));
            CustomCard.BuildCard<ShockwaveBounce>(ci => CardRegistry.Register("Shockwave Bounce", ci));
            CustomCard.BuildCard<IceRicochet>(ci => CardRegistry.Register("Ice Ricochet", ci));
            CustomCard.BuildCard<ScorchingBounce>(ci => CardRegistry.Register("Scorching Bounce", ci));

            // === Stat Boost Cards ===
            CustomCard.BuildCard<WideSpread>(ci => CardRegistry.Register("Wide Spread", ci));
            CustomCard.BuildCard<BulletSpeedPlus>(ci => CardRegistry.Register("Bullet Speed+", ci));
            CustomCard.BuildCard<RapidFire>(ci => CardRegistry.Register("Rapid Fire", ci));

            // === Ammo/Reload Cards ===
            CustomCard.BuildCard<InfiniteAmmo>(ci => CardRegistry.Register("Infinite Ammo", ci));
            CustomCard.BuildCard<QuickDraw>(ci => CardRegistry.Register("Quick Draw", ci));

            // === Unique Mechanic Cards ===
            CustomCard.BuildCard<BulletWarp>(ci => CardRegistry.Register("Bullet Warp", ci));
            CustomCard.BuildCard<SnakeRain>(ci => CardRegistry.Register("Snake Rain", ci));
            CustomCard.BuildCard<SpeedDemon>(ci => CardRegistry.Register("Speed Demon", ci));
            CustomCard.BuildCard<ChaosStats>(ci => CardRegistry.Register("Chaos Stats", ci));
            CustomCard.BuildCard<RisingTide>(ci => CardRegistry.Register("Rising Tide", ci));
            CustomCard.BuildCard<DoubleDown>(ci => CardRegistry.Register("Double Down", ci));
            CustomCard.BuildCard<AllIn>(ci => CardRegistry.Register("All In", ci));
            CustomCard.BuildCard<EffectAmplifier>(ci => CardRegistry.Register("Effect Amplifier", ci));

            // === Card Manipulation Cards ===
            CustomCard.BuildCard<NewHand>(ci => CardRegistry.Register("New Hand", ci));
            CustomCard.BuildCard<AllInOne>(ci => CardRegistry.Register("All-In-One", ci));
            CustomCard.BuildCard<CardThief>(ci => CardRegistry.Register("Card Thief", ci));
            CustomCard.BuildCard<ShuffleAndDeal>(ci => CardRegistry.Register("Shuffle & Deal", ci));
            CustomCard.BuildCard<CardTornado>(ci => CardRegistry.Register("Card Tornado", ci));
            CustomCard.BuildCard<HighStakes>(ci => CardRegistry.Register("High Stakes", ci));
            CustomCard.BuildCard<StatLeech>(ci => CardRegistry.Register("Stat Leech", ci));
            CustomCard.BuildCard<Copycat>(ci => CardRegistry.Register("Copycat", ci));

            // === Custom Projectile Cards ===
            CustomCard.BuildCard<AngryBirds>(ci => CardRegistry.Register("Angry Birds", ci));
            CustomCard.BuildCard<SquidInk>(ci => CardRegistry.Register("Squid Ink", ci));

            // === On-Block/On-Hit Spawner Cards ===
            CustomCard.BuildCard<NachoBlock>(ci => CardRegistry.Register("Nacho Block", ci));
            CustomCard.BuildCard<LemonDrop>(ci => CardRegistry.Register("Lemon Drop", ci));
            CustomCard.BuildCard<TortillaShield>(ci => CardRegistry.Register("Tortilla Shield", ci));

            // === Inert Replacement Cards (for card-swap safety) ===
            CustomCard.BuildCard<YayMyTurn>(ci => CardRegistry.Register("Yay It's My Turn!", ci));
            CustomCard.BuildCard<SharingIsCaring>(ci => CardRegistry.Register("Sharing is Caring", ci));
            CustomCard.BuildCard<NewHandPlaceholder>(ci => CardRegistry.Register("New Hand Used", ci));
            CustomCard.BuildCard<AllInOnePlaceholder>(ci => CardRegistry.Register("All-In-One Used", ci));
            CustomCard.BuildCard<CardThiefPlaceholder>(ci => CardRegistry.Register("Card Thief Used", ci));
            CustomCard.BuildCard<ShuffleDealPlaceholder>(ci => CardRegistry.Register("Shuffle & Deal Used", ci));
            CustomCard.BuildCard<CardTornadoPlaceholder>(ci => CardRegistry.Register("Card Tornado Used", ci));
            CustomCard.BuildCard<HighStakesPlaceholder>(ci => CardRegistry.Register("High Stakes Used", ci));
            CustomCard.BuildCard<StatLeechPlaceholder>(ci => CardRegistry.Register("Stat Leech Used", ci));
            CustomCard.BuildCard<CopycatPlaceholder>(ci => CardRegistry.Register("Copycat Used", ci));

            // === Debug/Test Cards ===
            CustomCard.BuildCard<OrbitalTest>(ci => CardRegistry.Register("Orbital Test", ci));

            // Class handlers (SaucyEnchiladasClass, ASourFruitClass, SynogenceClass) are
            // auto-discovered by ClassesManagerReborn via assembly reflection.

            Logger.LogInfo($"{ModName} v{Version} loaded! {CardRegistry.All.Count} cards registered across 3 classes.");
        }
    }
}
