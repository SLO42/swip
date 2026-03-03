using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class AngryBirds : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.7f;

            var e = player.gameObject.AddComponent<HomingBirdEffect>();
            e.birdCount = 2;
            e.birdDamage = 20f;
            e.birdSpeed = 12f;
            e.birdLifetime = 5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.7f;

            var e = player.gameObject.GetComponent<HomingBirdEffect>();
            if (e != null) Object.Destroy(e);
        }

        protected override string GetTitle() => "Angry Birds";
        protected override string GetDescription() => "They're not actually angry. They're FURIOUS.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Homing Birds", amount = "2 per shot", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-30%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
