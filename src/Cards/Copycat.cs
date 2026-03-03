using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class Copycat : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // Collect all cards from all players, excluding this card
            var allCards = new List<CardInfo>();
            foreach (var p in PlayerManager.instance.players)
            {
                foreach (var c in p.data.currentCards)
                {
                    if (c.cardName != "Copycat")
                    {
                        allCards.Add(c);
                    }
                }
            }

            if (allCards.Count == 0) return;

            var chosen = allCards[UnityEngine.Random.Range(0, allCards.Count)];
            int myCount = data.currentCards.Count;

            ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);

            for (int i = 0; i < myCount; i++)
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, chosen, false, "", 0f, 0f, true);
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("Copycat Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "Copycat")
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

        protected override string GetTitle() => "Copycat";
        protected override string GetDescription() => "Imitation is the sincerest form of theft.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Clone Any Card", amount = "For Entire Hand", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
