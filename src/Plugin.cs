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
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModId = "com.sam.rounds.swip";
        private const string ModName = "SWIP";
        private const string Version = "1.1.0";

        void Awake()
        {
            new Harmony(ModId).PatchAll();

            // Register custom Mythic rarity
            RarityLib.Utils.RarityUtils.AddRarity("Mythic", 0.02f, new Color(1f, 0.2f, 0.8f), new Color(0.6f, 0.1f, 0.5f));
        }

        void Start()
        {
            // Daniel's Cards (ASourFruit)
            CustomCard.BuildCard<SourPunch>();
            CustomCard.BuildCard<SourPatch>();
            CustomCard.BuildCard<OnePunchDan>();
            CustomCard.BuildCard<FruitSalad>();
            CustomCard.BuildCard<CitricAcid>();

            // Max's Cards (Synogence)
            CustomCard.BuildCard<TheAlgorithm>();
            CustomCard.BuildCard<SynapseFire>();
            CustomCard.BuildCard<ProtocolOverride>();
            CustomCard.BuildCard<BufferOverflow>();
            CustomCard.BuildCard<RecursiveLoop>();

            // Sam's Cards (SaucyEnchiladas)
            CustomCard.BuildCard<SpecialSauce>();
            CustomCard.BuildCard<ExtraSpicy>();
            CustomCard.BuildCard<EnchiladaTreatment>();
            CustomCard.BuildCard<SalsaVerde>();
            CustomCard.BuildCard<HotOnesChallenge>();

            // Brotherhood Cards
            CustomCard.BuildCard<WelcomeCard>();
            CustomCard.BuildCard<MomSaidMyTurn>();
            CustomCard.BuildCard<BrotherlyLove>();
            CustomCard.BuildCard<CouchCushionFort>();
            CustomCard.BuildCard<ControllerThrow>();
            CustomCard.BuildCard<LilBroEnergy>();
            CustomCard.BuildCard<BigBroEnergy>();
            CustomCard.BuildCard<SugarRush>();
            CustomCard.BuildCard<RageQuit>();
            CustomCard.BuildCard<NoU>();
            CustomCard.BuildCard<SnackBreak>();
            CustomCard.BuildCard<AmmoHoarder>();
            CustomCard.BuildCard<ComeHereBro>();
            CustomCard.BuildCard<ScorchedEarth>();
            CustomCard.BuildCard<WildCard>();

            // === Expansion 2: Chaos Arsenal ===

            // Daniel's New Cards (ASourFruit)
            CustomCard.BuildCard<AgentOrange>();
            CustomCard.BuildCard<ZyklonB>();
            CustomCard.BuildCard<LemonZest>();
            CustomCard.BuildCard<AcidRain>();

            // Max's New Cards (Synogence)
            CustomCard.BuildCard<LaserPrecision>();
            CustomCard.BuildCard<SatelliteUplink>();
            CustomCard.BuildCard<DroneSwarm>();
            CustomCard.BuildCard<Firewall>();

            // Sam's New Cards (SaucyEnchiladas)
            CustomCard.BuildCard<GhostPepper>();
            CustomCard.BuildCard<SmokeBomb>();
            CustomCard.BuildCard<Napalm>();

            // Brotherhood/Chaos Cards
            CustomCard.BuildCard<DivineJudgment>();
            CustomCard.BuildCard<CarpetBomb>();
            CustomCard.BuildCard<Smite>();
            CustomCard.BuildCard<ToxicRelationship>();
            CustomCard.BuildCard<Arsonist>();
            CustomCard.BuildCard<GasLeak>();
            CustomCard.BuildCard<HolyLight>();
            CustomCard.BuildCard<ScorchedSky>();
            CustomCard.BuildCard<TrailBlazer>();
            CustomCard.BuildCard<TheFinalBoss>();
            CustomCard.BuildCard<SnowballEffectCard>();

            // Stat Cards
            CustomCard.BuildCard<VitaminC>();
            CustomCard.BuildCard<Adrenaline>();
            CustomCard.BuildCard<ThickSkin>();
            CustomCard.BuildCard<GlassCannon>();

            // Bounce Cards
            CustomCard.BuildCard<RubberBullets>();
            CustomCard.BuildCard<Pinball>();
            CustomCard.BuildCard<BounceHouse>();

            // Healing Cards
            CustomCard.BuildCard<HealingMist>();


            Logger.LogInfo($"{ModName} v{Version} has loaded!");
        }
    }
}
