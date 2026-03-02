using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class ScorchedSky : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 1.3f;
            characterStats.movementSpeed *= 0.8f;

            // Orbital strike that triggers via shoot action (simulating "on kill" by firing on each shot)
            var orbital = player.gameObject.AddComponent<OrbitalStrikeEffect>();
            orbital.explosionCount = 2;
            orbital.explosionDamage = 40f;
            orbital.explosionRange = 3.5f;
            orbital.triggerOnBlock = true;
            orbital.triggerOnKill = true;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 1.3f;
            characterStats.movementSpeed /= 0.8f;

            var orbital = player.gameObject.GetComponent<OrbitalStrikeEffect>();
            if (orbital != null) Object.Destroy(orbital);
        }

        protected override string GetTitle() => "Scorched Sky";
        protected override string GetDescription() => "Nothing is safe. Not even the sky.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Orbital Strike", amount = "On Block", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Damage", amount = "+30%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Speed", amount = "-20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
