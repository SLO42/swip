using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class SlowTide : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var s = player.gameObject.AddComponent<SlowOnHitSpawner>();
            s.slowPercent = 0.5f;
            s.slowDuration = 4f;
            gun.damage *= 0.8f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var s = player.gameObject.GetComponent<SlowOnHitSpawner>();
            if (s != null) Object.Destroy(s);
            gun.damage /= 0.8f;
        }

        protected override string GetTitle() => "Slow Tide";
        protected override string GetDescription() => "The world moves at your pace now.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Slow", amount = "50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Duration", amount = "4s", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.ColdBlue;
        public override string GetModName() => "SWIP";
    }
}
