using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class Smite : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.healing += 30f;
            characterStats.movementSpeed *= 0.7f;

            var spawner = player.gameObject.AddComponent<SmiteBeamSpawner>();
            spawner.beamDamage = 55f;
            spawner.beamWidth = 1.5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.healing -= 30f;
            characterStats.movementSpeed /= 0.7f;

            var spawner = player.gameObject.GetComponent<SmiteBeamSpawner>();
            if (spawner != null) Object.Destroy(spawner);
        }

        protected override string GetTitle() => "Smite";
        protected override string GetDescription() => "Divine punishment on impact.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Holy Beam", amount = "On Hit", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Block Heal", amount = "+30", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Speed", amount = "-30%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
