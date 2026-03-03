using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class BulletSpeedPlus : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.projectileSpeed *= 1.5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.projectileSpeed /= 1.5f;
        }

        protected override string GetTitle() => "Bullet Speed+";
        protected override string GetDescription() => "Zoom.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Bullet Speed", amount = "+50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
