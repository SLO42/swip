using RarityLib.Utils;
using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class Contagion : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles += 1;
            gun.spread *= 1.3f;
            characterStats.movementSpeed *= 0.9f;

            var cloud = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloud.cloudRadius = 3f;
            cloud.cloudDuration = 4f;
            cloud.damagePerSecond = 5f;
            cloud.outerColor = new Color(0.3f, 0.7f, 0.1f, 0.5f);
            cloud.innerColor = new Color(0.4f, 0.8f, 0.2f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles -= 1;
            gun.spread /= 1.3f;
            characterStats.movementSpeed /= 0.9f;

            var cloud = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloud != null) Object.Destroy(cloud);
        }

        protected override string GetTitle() => "Contagion";
        protected override string GetDescription() => "It spreads. They all fall eventually.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Poison Cloud",
                    amount = "5 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullets",
                    amount = "+1",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Spread",
                    amount = "+30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Movement Speed",
                    amount = "-10%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
