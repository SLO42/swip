using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class CorrosiveSpray : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles += 3;
            gun.damage *= 0.5f;
            gun.spread *= 2f;

            var cloud = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloud.cloudRadius = 2f;
            cloud.cloudDuration = 3f;
            cloud.damagePerSecond = 5f;
            cloud.outerColor = new Color(0.2f, 0.8f, 0.1f, 0.5f);
            cloud.innerColor = new Color(0.3f, 0.9f, 0.2f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles -= 3;
            gun.damage /= 0.5f;
            gun.spread /= 2f;

            var cloud = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloud != null) Object.Destroy(cloud);
        }

        protected override string GetTitle() => "Corrosive Spray";
        protected override string GetDescription() => "Three streams of corrosive mist.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullets",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Spread",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Poison Cloud",
                    amount = "5 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
