using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class NewHand : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            int cardCount = data.currentCards.Count;
            ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);

            for (int i = 0; i < cardCount; i++)
            {
                var random = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(
                    player, gun, gunAmmo, data, health, gravity, block, characterStats,
                    (ci, p, g, ga, d, h, gr, b, cs) => true
                );
                if (random != null)
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, random, false, "", 0f, 0f, true);
                }
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("New Hand Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "New Hand")
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

        protected override string GetTitle() => "New Hand";
        protected override string GetDescription() => "Throw away everything. Start fresh. Regret nothing.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Replace All Cards", amount = "Random Hand", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
