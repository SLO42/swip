using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class SquidInk : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.75f;

            var e = player.gameObject.AddComponent<HomingSquidEffect>();
            e.squidCount = 2;
            e.squidDamage = 15f;
            e.squidSpeed = 8f;
            e.squidLifetime = 6f;
            e.inkRadius = 3f;
            e.inkDuration = 3f;
            e.inkSlowAmount = 0.5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.75f;

            var e = player.gameObject.GetComponent<HomingSquidEffect>();
            if (e != null) Object.Destroy(e);
        }

        protected override string GetTitle() => "Squid Ink";
        protected override string GetDescription() => "Swim, little squids. SWIM.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Homing Squids", amount = "2 per shot", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Ink Cloud", amount = "Slow enemies", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
