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
            // Orbital strike on bullet hit via SmiteOnHitSpawner
            var smite = player.gameObject.AddComponent<SmiteOnHitSpawner>();
            smite.explosionCount = 3;
            smite.explosionDamage = 40f;
            smite.explosionRange = 3f;
            smite.delayBetween = 0.15f;

            // Also trigger orbital strike on block
            var orbital = player.gameObject.AddComponent<OrbitalStrikeEffect>();
            orbital.explosionCount = 2;
            orbital.explosionDamage = 35f;
            orbital.explosionRange = 3f;
            orbital.triggerOnBlock = true;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var smite = player.gameObject.GetComponent<SmiteOnHitSpawner>();
            if (smite != null) Object.Destroy(smite);

            var orbital = player.gameObject.GetComponent<OrbitalStrikeEffect>();
            if (orbital != null) Object.Destroy(orbital);
        }

        protected override string GetTitle() => "Orbital Test";
        protected override string GetDescription() => "DEBUG: Strike on hit + block.";

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
