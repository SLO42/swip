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

            var sky = player.gameObject.AddComponent<ScorchedSkySpawner>();
            sky.projectileCount = 5;
            sky.spreadWidth = 6f;
            sky.fallSpeed = 40f;
            sky.projectileDamage = 25f;
            sky.explosionRange = 2f;
            sky.explosionForce = 1500f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 1.3f;
            characterStats.movementSpeed /= 0.8f;

            var sky = player.gameObject.GetComponent<ScorchedSkySpawner>();
            if (sky != null) Object.Destroy(sky);
        }

        protected override string GetTitle() => "Scorched Sky";
        protected override string GetDescription() => "Nothing is safe. Not even the sky.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Sky Rain", amount = "5 On Hit", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
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
