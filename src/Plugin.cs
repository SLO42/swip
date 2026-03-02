using BepInEx;
using HarmonyLib;
using UnboundLib.Cards;
using UnityEngine;
using SWIP.Cards;

namespace SWIP
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.rarity.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModId = "com.sam.rounds.swip";
        private const string ModName = "SWIP";
        private const string Version = "2.0.0";

        void Awake()
        {
            new Harmony(ModId).PatchAll();

            // Register custom rarities
            RarityLib.Utils.RarityUtils.AddRarity("Epic", 0.08f, new Color(0.6f, 0.2f, 0.8f), new Color(0.4f, 0.1f, 0.5f));
            RarityLib.Utils.RarityUtils.AddRarity("Legendary", 0.05f, new Color(1f, 0.84f, 0f), new Color(0.8f, 0.6f, 0f));
            RarityLib.Utils.RarityUtils.AddRarity("Mythic", 0.02f, new Color(1f, 0.2f, 0.8f), new Color(0.6f, 0.1f, 0.5f));
        }

        void Start()
        {
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
            CustomCard.BuildCard<ScorchedSky>(ci => CardRegistry.Register("Scorched Sky", ci));
            CustomCard.BuildCard<MomSaidMyTurn>(ci => CardRegistry.Register("Mom Said My Turn", ci));
            CustomCard.BuildCard<BrotherlyLove>(ci => CardRegistry.Register("Brotherly Love", ci));
            CustomCard.BuildCard<DivineJudgment>(ci => CardRegistry.Register("Divine Judgment", ci));
            CustomCard.BuildCard<CarpetBomb>(ci => CardRegistry.Register("Carpet Bomb", ci));
            CustomCard.BuildCard<SatelliteUplink>(ci => CardRegistry.Register("Satellite Uplink", ci));
            CustomCard.BuildCard<DroneSwarm>(ci => CardRegistry.Register("Drone Swarm", ci));
            CustomCard.BuildCard<Underdog>(ci => CardRegistry.Register("Underdog", ci));
            CustomCard.BuildCard<MomentumCard>(ci => CardRegistry.Register("Momentum", ci));
            CustomCard.BuildCard<Berserker>(ci => CardRegistry.Register("Berserker", ci));
            CustomCard.BuildCard<ExtraLife>(ci => CardRegistry.Register("Extra Life", ci));
            CustomCard.BuildCard<LastStand>(ci => CardRegistry.Register("Last Stand", ci));
            CustomCard.BuildCard<Nullifier>(ci => CardRegistry.Register("Nullifier", ci));
            CustomCard.BuildCard<TheFinalBoss>(ci => CardRegistry.Register("The Final Boss", ci));

            // Class handlers (SaucyEnchiladasClass, ASourFruitClass, SynogenceClass) are
            // auto-discovered by ClassesManagerReborn via assembly reflection.

            Logger.LogInfo($"{ModName} v{Version} loaded! {CardRegistry.All.Count} cards registered across 3 classes.");
        }
    }
}
