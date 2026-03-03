using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class TortillaShield : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var e = player.gameObject.AddComponent<TortillaWrapEffect>();
            e.wrapDuration = 3f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var e = player.gameObject.GetComponent<TortillaWrapEffect>();
            if (e != null) Object.Destroy(e);
        }

        protected override string GetTitle() => "Tortilla Shield";
        protected override string GetDescription() => "Wrap yourself in carbs. Become invincible.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Invulnerable Wrap", amount = "On Block", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Duration", amount = "3s", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Immobile", amount = "During Wrap", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DefensiveBlue;
        public override string GetModName() => "SWIP";
    }
}
