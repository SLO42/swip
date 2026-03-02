using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class SaucyEnchiladasEntry : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 1.15f;
            characterStats.regen += 5f;
            characterStats.lifeSteal += 0.05f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 1.15f;
            characterStats.regen -= 5f;
            characterStats.lifeSteal -= 0.05f;
        }

        protected override string GetTitle() => "SaucyEnchiladas Entry";
        protected override string GetDescription() => "Sustain through gas and area denial. Poison your enemies, heal your wounds.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "HP", amount = "+15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Regen", amount = "+5", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Life Steal", amount = "+5%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
