using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;

namespace SWIP.Cards
{
    public class MomSaidMyTurn : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            int numCards = UnityEngine.Random.Range(3, 6);
            for (int i = 0; i < numCards; i++)
            {
                var randomCard = ModdingUtils.Utils.Cards.instance.GetRandomCardWithCondition(
                    player,
                    gun,
                    gunAmmo,
                    data,
                    health,
                    gravity,
                    block,
                    characterStats,
                    (cardInfo2, p, g, gA, cData, hHandler, grav, blk, cStats) => true
                );
                if (randomCard != null)
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 0f, 0f, true);
                }
            }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle() => "Mom Said It's My Turn";
        protected override string GetDescription() => "Pull the lever. Pray to the algorithm.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Random Cards", amount = "3-5", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
