using RarityLib.Utils;
using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class PlagueDoctor : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 0.8f;
            characterStats.regen += 8f;

            var zone = player.gameObject.AddComponent<ZoneBehaviour>();
            zone.radius = 4f;
            zone.duration = float.MaxValue;
            zone.damagePerSecond = 6f;
            zone.followTarget = player.transform;
            zone.owner = player;
            zone.affectsOwner = false;
            zone.outerColor = new Color(0.5f, 0.2f, 0.8f, 0.5f);
            zone.innerColor = new Color(0.6f, 0.3f, 0.9f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 0.8f;
            characterStats.regen -= 8f;

            var zone = player.gameObject.GetComponent<ZoneBehaviour>();
            if (zone != null) Object.Destroy(zone);
        }

        protected override string GetTitle() => "Plague Doctor";
        protected override string GetDescription() => "Heals the faithful. Poisons the rest.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Poison Aura",
                    amount = "6 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Regeneration",
                    amount = "+8 HP/s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Health",
                    amount = "-20%",
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
