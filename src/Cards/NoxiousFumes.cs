using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class NoxiousFumes : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.8f;
            gun.projectielSimulatonSpeed *= 1.2f;

            var cloud = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloud.cloudRadius = 2f;
            cloud.cloudDuration = 4f;
            cloud.damagePerSecond = 4f;
            cloud.outerColor = new Color(0.2f, 0.7f, 0.1f, 0.5f);
            cloud.innerColor = new Color(0.3f, 0.8f, 0.2f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.8f;
            gun.projectielSimulatonSpeed /= 1.2f;

            var cloud = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloud != null) Object.Destroy(cloud);
        }

        protected override string GetTitle() => "Noxious Fumes";
        protected override string GetDescription() => "The air itself becomes toxic.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
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
                    stat = "Bullet Speed",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Poison Cloud",
                    amount = "4 DPS",
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
