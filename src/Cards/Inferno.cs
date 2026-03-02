using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class Inferno : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.7f;
            gun.projectileSpeed *= 1.2f;

            var burn = player.gameObject.AddComponent<BurnEffectSpawner>();
            burn.burnDPS = 15f;
            burn.burnDuration = 6f;

            var cloud = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloud.cloudRadius = 3f;
            cloud.cloudDuration = 5f;
            cloud.damagePerSecond = 6f;
            cloud.outerColor = new Color(1f, 0.3f, 0.1f, 0.5f);
            cloud.innerColor = new Color(1f, 0.5f, 0.2f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.7f;
            gun.projectileSpeed /= 1.2f;

            var burn = player.gameObject.GetComponent<BurnEffectSpawner>();
            if (burn != null) Object.Destroy(burn);

            var cloud = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloud != null) Object.Destroy(cloud);
        }

        protected override string GetTitle() => "Inferno";
        protected override string GetDescription() => "Everything burns. The trail, the cloud, the aftermath.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullet Speed",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Burn",
                    amount = "15 DPS, 6s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Fire Cloud",
                    amount = "6 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
