using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class Biohazard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 0.75f;

            var zone = player.gameObject.AddComponent<ZoneBehaviour>();
            zone.radius = 5f;
            zone.duration = float.MaxValue;
            zone.damagePerSecond = 8f;
            zone.slowAmount = 0.2f;
            zone.followTarget = player.transform;
            zone.owner = player;
            zone.affectsOwner = false;
            zone.outerColor = new Color(0.6f, 0.8f, 0.1f, 0.5f);
            zone.innerColor = new Color(0.7f, 0.9f, 0.2f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 0.75f;

            var zone = player.gameObject.GetComponent<ZoneBehaviour>();
            if (zone != null) Object.Destroy(zone);
        }

        protected override string GetTitle() => "Biohazard";
        protected override string GetDescription() => "Walking biohazard. Stay back.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Toxic Aura",
                    amount = "8 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Aura Slow",
                    amount = "20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Health",
                    amount = "-25%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
