using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class QuickDraw : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reloadTime *= 0.01f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reloadTime /= 0.01f;
        }

        protected override string GetTitle() => "Quick Draw";
        protected override string GetDescription() => "Reload? What reload?";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Reload Time", amount = "x0.01", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
