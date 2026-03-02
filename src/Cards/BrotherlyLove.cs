using System.Collections.Generic;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Utils;

namespace SWIP.Cards
{
    public class BrotherlyLove : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var players = PlayerManager.instance.players;
            if (players.Count < 2) return;

            List<List<CardInfo>> allCards = new List<List<CardInfo>>();
            foreach (var p in players)
            {
                allCards.Add(new List<CardInfo>(p.data.currentCards));
            }

            foreach (var p in players)
            {
                ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(p);
            }

            for (int i = 0; i < players.Count; i++)
            {
                int sourceIndex = (i + 1) % players.Count;
                foreach (var card in allCards[sourceIndex])
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(players[i], card, false, "", 0f, 0f, true);
                }
            }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle() => "Brotherly Love";
        protected override string GetDescription() => "When the music stops, nobody knows what they have.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Card Rotation", amount = "All Players", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
