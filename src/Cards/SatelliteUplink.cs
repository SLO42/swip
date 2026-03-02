using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class SatelliteUplink : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.additionalBlocks += 1;
            data.maxHealth *= 0.75f;

            var orbital = player.gameObject.AddComponent<OrbitalStrikeEffect>();
            orbital.explosionCount = 3;
            orbital.explosionDamage = 55f;
            orbital.explosionRange = 4f;
            orbital.triggerOnBlock = true;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.additionalBlocks -= 1;
            data.maxHealth /= 0.75f;

            var orbital = player.gameObject.GetComponent<OrbitalStrikeEffect>();
            if (orbital != null) Object.Destroy(orbital);
        }

        protected override string GetTitle() => "Satellite Uplink";
        protected override string GetDescription() => "Requesting orbital support.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Orbital Strike", amount = "On Block", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Blocks", amount = "+1", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "HP", amount = "-25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
