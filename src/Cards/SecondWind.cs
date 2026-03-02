using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class SecondWind : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.cdMultiplier *= 0.7f;
            characterStats.movementSpeed *= 1.15f;
            block.healing += 30f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.cdMultiplier /= 0.7f;
            characterStats.movementSpeed /= 1.15f;
            block.healing -= 30f;
        }

        protected override string GetTitle() => "Second Wind";
        protected override string GetDescription() => "Catch your breath. Then run.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Block Cooldown",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Movement Speed",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Heal on Block",
                    amount = "+30",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
