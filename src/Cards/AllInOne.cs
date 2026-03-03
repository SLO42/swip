using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class AllInOne : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (data.currentCards.Count == 0) return;

            var chosen = data.currentCards[UnityEngine.Random.Range(0, data.currentCards.Count)];
            int count = data.currentCards.Count;

            ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);

            for (int i = 0; i < count; i++)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, chosen, false, "", 0f, 0f, true);
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("All-In-One Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "All-In-One")
                    {
                        data.currentCards[j] = ph;
                        break;
                    }
                }
            }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle() => "All-In-One";
        protected override string GetDescription() => "One card to rule them all.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "All Cards Become", amount = "One", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
