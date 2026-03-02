using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class NuclearOption : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 3;
            gun.damage *= 1.5f;
            gun.attackSpeed *= 2f;
            gun.projectileSize *= 1.5f;

            var explodeSpawner = player.gameObject.AddComponent<ExplodingBulletSpawner>();
            explodeSpawner.explosionDamage = 150f;
            explodeSpawner.explosionRange = 6f;
            explodeSpawner.explosionForce = 4000f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 3;
            gun.damage /= 1.5f;
            gun.attackSpeed /= 2f;
            gun.projectileSize /= 1.5f;

            var explodeSpawner = player.gameObject.GetComponent<ExplodingBulletSpawner>();
            if (explodeSpawner != null) Object.Destroy(explodeSpawner);
        }

        protected override string GetTitle() => "Nuclear Option";
        protected override string GetDescription() => "The last bounce is the one that matters.";
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
                    positive = true,
                    stat = "Damage",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Attack speed",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullet size",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Explosion on hit",
                    amount = "150 dmg",
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
