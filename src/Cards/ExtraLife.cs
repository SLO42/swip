using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class ExtraLife : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.respawns += 1;
            characterStats.movementSpeed *= 0.9f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.respawns -= 1;
            characterStats.movementSpeed /= 0.9f;
        }

        protected override string GetTitle() => "Extra Life";
        protected override string GetDescription() => "One more chance. Don't waste it.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Extra Life", amount = "+1", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Speed", amount = "-10%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityLib.Utils.RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
