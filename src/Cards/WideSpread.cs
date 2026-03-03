using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class WideSpread : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.spread += 0.5f;
            gun.numberOfProjectiles += 2;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.spread -= 0.5f;
            gun.numberOfProjectiles -= 2;
        }

        protected override string GetTitle() => "Wide Spread";
        protected override string GetDescription() => "Shotgun diplomacy.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Spread", amount = "+0.5", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Bullets", amount = "+2", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
