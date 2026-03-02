using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class VolatilePayload : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 3;
            gun.damage *= 0.8f;

            var growSpawner = player.gameObject.AddComponent<GrowOnBounceSpawner>();
            growSpawner.growthFactor = 1.4f;

            var explodeSpawner = player.gameObject.AddComponent<ExplodingBulletSpawner>();
            explodeSpawner.explosionDamage = 40f;
            explodeSpawner.explosionRange = 3f;
            explodeSpawner.explosionForce = 1500f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 3;
            gun.damage /= 0.8f;

            var growSpawner = player.gameObject.GetComponent<GrowOnBounceSpawner>();
            if (growSpawner != null) Object.Destroy(growSpawner);

            var explodeSpawner = player.gameObject.GetComponent<ExplodingBulletSpawner>();
            if (explodeSpawner != null) Object.Destroy(explodeSpawner);
        }

        protected override string GetTitle() => "Volatile Payload";
        protected override string GetDescription() => "Bigger. Angrier. More explosive.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bounces",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Growth per bounce",
                    amount = "+40%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Explosion on hit",
                    amount = "40 dmg",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
