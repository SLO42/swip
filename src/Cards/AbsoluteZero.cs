using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class AbsoluteZero : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.movementSpeed *= 0.7f;
            data.maxHealth *= 1.2f;

            var zoneObj = new GameObject("FreezeAura");
            zoneObj.transform.SetParent(player.transform);
            zoneObj.transform.localPosition = Vector3.zero;
            var zone = zoneObj.AddComponent<ZoneBehaviour>();
            zone.radius = 5f;
            zone.duration = float.MaxValue;
            zone.slowAmount = 0.6f;
            zone.damagePerSecond = 3f;
            zone.owner = player;
            zone.affectsOwner = false;
            zone.followTarget = player.transform;
            zone.outerColor = new Color(0.1f, 0.2f, 0.8f, 0.5f);
            zone.innerColor = new Color(0.2f, 0.4f, 1f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.movementSpeed /= 0.7f;
            data.maxHealth /= 1.2f;

            var zoneObj = player.transform.Find("FreezeAura");
            if (zoneObj != null) Object.Destroy(zoneObj.gameObject);
        }

        protected override string GetTitle() => "Absolute Zero";
        protected override string GetDescription() => "All motion ceases. Including yours.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Freeze Aura",
                    amount = "60% slow, 3 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Health",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Movement Speed",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
