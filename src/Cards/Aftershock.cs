using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class Aftershock : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 2;
            gun.damage *= 1.2f;
            gun.attackSpeed *= 1.5f;

            var explodeSpawner = player.gameObject.AddComponent<ExplodingBulletSpawner>();
            explodeSpawner.explosionDamage = 40f;
            explodeSpawner.explosionRange = 3f;
            explodeSpawner.explosionForce = 1200f;

            var cloudSpawner = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloudSpawner.cloudRadius = 3f;
            cloudSpawner.cloudDuration = 4f;
            cloudSpawner.damagePerSecond = 5f;
            cloudSpawner.outerColor = new Color(1f, 0.5f, 0.1f, 0.5f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 2;
            gun.damage /= 1.2f;
            gun.attackSpeed /= 1.5f;

            var explodeSpawner = player.gameObject.GetComponent<ExplodingBulletSpawner>();
            if (explodeSpawner != null) Object.Destroy(explodeSpawner);

            var cloudSpawner = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloudSpawner != null) Object.Destroy(cloudSpawner);
        }

        protected override string GetTitle() => "Aftershock";
        protected override string GetDescription() => "The explosion is just the beginning.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bounces",
                    amount = "+2",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Attack speed",
                    amount = "-33%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Explosion on hit",
                    amount = "40 dmg",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Toxic cloud",
                    amount = "5 dps",
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
