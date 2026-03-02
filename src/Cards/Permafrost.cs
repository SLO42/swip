using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class Permafrost : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.75f;

            var freeze = player.gameObject.AddComponent<FreezeEffectSpawner>();
            freeze.slowPercent = 0.6f;
            freeze.freezeDuration = 5f;

            var cloud = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloud.cloudRadius = 3f;
            cloud.cloudDuration = 5f;
            cloud.damagePerSecond = 4f;
            cloud.slowAmount = 0.3f;
            cloud.outerColor = new Color(0.3f, 0.6f, 1f, 0.5f);
            cloud.innerColor = new Color(0.5f, 0.8f, 1f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.75f;

            var freeze = player.gameObject.GetComponent<FreezeEffectSpawner>();
            if (freeze != null) Object.Destroy(freeze);

            var cloud = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloud != null) Object.Destroy(cloud);
        }

        protected override string GetTitle() => "Permafrost";
        protected override string GetDescription() => "A frozen wasteland wherever your bullets land.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-25%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Freeze",
                    amount = "60% slow, 5s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Frost Cloud",
                    amount = "4 DPS, 30% slow",
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
