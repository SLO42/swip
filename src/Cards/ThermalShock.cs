using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class ThermalShock : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 1.15f;
            gun.attackSpeed *= 1.3f;

            var burn = player.gameObject.AddComponent<BurnEffectSpawner>();
            burn.burnDPS = 8f;
            burn.burnDuration = 4f;

            var freeze = player.gameObject.AddComponent<FreezeEffectSpawner>();
            freeze.slowPercent = 0.4f;
            freeze.freezeDuration = 3f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 1.15f;
            gun.attackSpeed /= 1.3f;

            var burn = player.gameObject.GetComponent<BurnEffectSpawner>();
            if (burn != null) Object.Destroy(burn);

            var freeze = player.gameObject.GetComponent<FreezeEffectSpawner>();
            if (freeze != null) Object.Destroy(freeze);
        }

        protected override string GetTitle() => "Thermal Shock";
        protected override string GetDescription() => "Fire and ice at once. Nothing survives the transition.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Attack Speed",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Burn",
                    amount = "8 DPS, 4s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Freeze",
                    amount = "40% slow, 3s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
