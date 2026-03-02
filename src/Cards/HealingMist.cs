using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class HealingMist : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.8f;
            data.maxHealth *= 1.2f;

            var effect = player.gameObject.AddComponent<HealingMistEffect>();
            effect.cloudRadius = 3f;
            effect.cloudDuration = 5f;
            effect.healPerSecond = 15f;
            effect.outerColor = new Color(0.3f, 0.9f, 0.5f, 0.5f);
            effect.innerColor = new Color(0.5f, 1f, 0.7f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.8f;
            data.maxHealth /= 1.2f;

            var effect = player.gameObject.GetComponent<HealingMistEffect>();
            if (effect != null) Object.Destroy(effect);
        }

        protected override string GetTitle() => "Healing Mist";
        protected override string GetDescription() => "Block to release a soothing cloud.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Healing Cloud", amount = "On Block", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Cloud Heal/s", amount = "15", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Cloud Duration", amount = "5s", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Health", amount = "+20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DefensiveBlue;
        public override string GetModName() => "SWIP";
    }
}
