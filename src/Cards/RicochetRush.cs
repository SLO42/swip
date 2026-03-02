using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class RicochetRush : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 4;
            gun.damage *= 0.75f;

            var statSpawner = player.gameObject.AddComponent<StatOnBounceSpawner>();
            statSpawner.damageMultPerBounce = 1.2f;
            statSpawner.speedMultPerBounce = 1.15f;

            var explodeSpawner = player.gameObject.AddComponent<ExplodingBulletSpawner>();
            explodeSpawner.explosionDamage = 30f;
            explodeSpawner.explosionRange = 2f;
            explodeSpawner.explosionForce = 1000f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 4;
            gun.damage /= 0.75f;

            var statSpawner = player.gameObject.GetComponent<StatOnBounceSpawner>();
            if (statSpawner != null) Object.Destroy(statSpawner);

            var explodeSpawner = player.gameObject.GetComponent<ExplodingBulletSpawner>();
            if (explodeSpawner != null) Object.Destroy(explodeSpawner);
        }

        protected override string GetTitle() => "Ricochet Rush";
        protected override string GetDescription() => "Bounce. Boom. Repeat.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bounces",
                    amount = "+4",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-25%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage per bounce",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Speed per bounce",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Explosion on hit",
                    amount = "30 dmg",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
