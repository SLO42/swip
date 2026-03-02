using RarityLib.Utils;
using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class LifeDrain : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.lifeSteal += 0.2f;
            characterStats.movementSpeed *= 0.85f;

            var zone = player.gameObject.AddComponent<ZoneBehaviour>();
            zone.radius = 4f;
            zone.duration = float.MaxValue;
            zone.damagePerSecond = 6f;
            zone.followTarget = player.transform;
            zone.owner = player;
            zone.affectsOwner = false;
            zone.outerColor = new Color(0.4f, 0.1f, 0.4f, 0.5f);
            zone.innerColor = new Color(0.5f, 0.2f, 0.5f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.lifeSteal -= 0.2f;
            characterStats.movementSpeed /= 0.85f;

            var zone = player.gameObject.GetComponent<ZoneBehaviour>();
            if (zone != null) Object.Destroy(zone);
        }

        protected override string GetTitle() => "Life Drain";
        protected override string GetDescription() => "Their pain is your medicine.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage Aura",
                    amount = "6 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Life Steal",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Movement Speed",
                    amount = "-15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
