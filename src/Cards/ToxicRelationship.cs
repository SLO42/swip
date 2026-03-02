using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class ToxicRelationship : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.lifeSteal += 0.2f;
            data.maxHealth *= 0.75f;
            characterStats.movementSpeed *= 0.85f;

            var zone = player.gameObject.AddComponent<ZoneBehaviour>();
            zone.radius = 4f;
            zone.duration = float.MaxValue;
            zone.damagePerSecond = 12f;
            zone.owner = player;
            zone.outerColor = new Color(0.6f, 0.2f, 0.8f, 0.5f);
            zone.innerColor = new Color(0.7f, 0.3f, 0.9f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.lifeSteal -= 0.2f;
            data.maxHealth /= 0.75f;
            characterStats.movementSpeed /= 0.85f;

            var zone = player.gameObject.GetComponent<ZoneBehaviour>();
            if (zone != null) Object.Destroy(zone);
        }

        protected override string GetTitle() => "Toxic Relationship";
        protected override string GetDescription() => "It's complicated.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Poison Aura", amount = "Constant", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Life Steal", amount = "+20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "HP", amount = "-25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Speed", amount = "-15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
