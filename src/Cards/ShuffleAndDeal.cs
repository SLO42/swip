using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class ShuffleAndDeal : CustomCard
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
                int count = other.data.currentCards.Count;
                if (count == 0) continue;

                var otherGun = other.GetComponentInChildren<Gun>();
                var otherAmmo = other.GetComponentInChildren<GunAmmo>();

                ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(other);

                for (int i = 0; i < count; i++)
                {
                    var random = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(
                        other, otherGun, otherAmmo, other.data, other.data.healthHandler,
                        other.GetComponent<Gravity>(), other.GetComponent<Block>(), other.data.stats,
                        (ci, p, g, ga, d, h, gr, b, cs) => true
                    );
                    if (random != null)
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(other, random, false, "", 0f, 0f, true);
                    }
                }
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("Shuffle & Deal Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "Shuffle & Deal")
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

        protected override string GetTitle() => "Shuffle & Deal";
        protected override string GetDescription() => "Dealer takes all. And redistributes poorly.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Others Shuffle", amount = "All Cards", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
