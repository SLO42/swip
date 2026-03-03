using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;

namespace SWIP.Cards
{
    public class CardThief : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var others = PlayerManager.instance.players.FindAll(p => p != player && !p.data.dead);

            foreach (var other in others)
            {
                if (other.data.currentCards.Count == 0) continue;

                int idx = UnityEngine.Random.Range(0, other.data.currentCards.Count);
                var random = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(
                    player, gun, gunAmmo, data, health, gravity, block, characterStats,
                    (ci, p, g, ga, d, h, gr, b, cs) => true
                );
                if (random != null)
                {
                    other.data.currentCards[idx] = random;
                }
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("Card Thief Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "Card Thief")
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

        protected override string GetTitle() => "Card Thief";
        protected override string GetDescription() => "What's yours is... also yours, but different.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Others Swap", amount = "1 Random Card", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
