using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class Singularity : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 5;
            gun.damage *= 2f;
            gun.attackSpeed *= 3f;
            gun.ammo -= 2;
            gun.projectileSize *= 2f;

            var explodeSpawner = player.gameObject.AddComponent<ExplodingBulletSpawner>();
            explodeSpawner.explosionDamage = 200f;
            explodeSpawner.explosionRange = 8f;
            explodeSpawner.explosionForce = 6000f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 5;
            gun.damage /= 2f;
            gun.attackSpeed /= 3f;
            gun.ammo += 2;
            gun.projectileSize /= 2f;

            var explodeSpawner = player.gameObject.GetComponent<ExplodingBulletSpawner>();
            if (explodeSpawner != null) Object.Destroy(explodeSpawner);
        }

        protected override string GetTitle() => "Singularity";
        protected override string GetDescription() => "A point of infinite density. Nothing escapes.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bounces",
                    amount = "+5",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Attack speed",
                    amount = "-67%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Ammo",
                    amount = "-2",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullet size",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Explosion on hit",
                    amount = "200 dmg",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
