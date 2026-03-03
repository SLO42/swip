using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class OrbitalTest : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // Holy beam on bullet hit
            var smite = player.gameObject.AddComponent<SmiteBeamSpawner>();
            smite.beamDamage = 40f;
            smite.beamWidth = 1.2f;

            // Satellite laser on block
            var laser = player.gameObject.AddComponent<SatelliteLaserEffect>();
            laser.laserDamage = 50f;
            laser.laserWidth = 0.6f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var smite = player.gameObject.GetComponent<SmiteBeamSpawner>();
            if (smite != null) Object.Destroy(smite);

            var laser = player.gameObject.GetComponent<SatelliteLaserEffect>();
            if (laser != null) Object.Destroy(laser);
        }

        protected override string GetTitle() => "Orbital Test";
        protected override string GetDescription() => "DEBUG: Beam on hit + laser on block.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Strike", amount = "On Hit", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Strike", amount = "On Block", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
