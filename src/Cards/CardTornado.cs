using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class CardTornado : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var allPlayers = PlayerManager.instance.players;
            var pool = new List<CardInfo>();
            var counts = new int[allPlayers.Count];

            for (int i = 0; i < allPlayers.Count; i++)
            {
                counts[i] = allPlayers[i].data.currentCards.Count;
                foreach (var card in allPlayers[i].data.currentCards)
                {
                    // Skip this card to avoid recursive manipulation
                    if (card.cardName != "Card Tornado")
                    {
                        pool.Add(card);
                    }
                }
            }

            // Fisher-Yates shuffle
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                var temp = pool[i];
                pool[i] = pool[j];
                pool[j] = temp;
            }

            // Remove all cards from all players
            foreach (var p in allPlayers)
            {
                ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(p);
            }

            // Redistribute from the shuffled pool
            int poolIdx = 0;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                for (int j = 0; j < counts[i] && poolIdx < pool.Count; j++)
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(allPlayers[i], pool[poolIdx], false, "", 0f, 0f, true);
                    poolIdx++;
                }
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("Card Tornado Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "Card Tornado")
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

        protected override string GetTitle() => "Card Tornado";
        protected override string GetDescription() => "Cards go up. Cards come down. Nobody knows whose is whose.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Pool & Redistribute", amount = "All Cards", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
