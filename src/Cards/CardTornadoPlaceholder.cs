using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;

namespace SWIP.Cards
{
    /// <summary>
    /// Inert placeholder card that replaces "Card Tornado" after execution,
    /// preventing it from re-triggering on swap.
    /// </summary>
    public class CardTornadoPlaceholder : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // Intentionally empty — this card does nothing
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle() => "Card Tornado Used";
        protected override string GetDescription() => "The tornado has passed. Pick up the pieces.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Redistributed", amount = "Cards", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
