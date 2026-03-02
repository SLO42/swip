using RarityLib.Utils;
using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class DeadZone : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 1.3f;
            characterStats.movementSpeed *= 0.85f;
            gun.damage *= 0.8f;

            var effect = player.gameObject.AddComponent<DeadZoneEffect>();
            effect.player = player;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 1.3f;
            characterStats.movementSpeed /= 0.85f;
            gun.damage /= 0.8f;

            var effect = player.gameObject.GetComponent<DeadZoneEffect>();
            if (effect != null) Object.Destroy(effect);
        }

        protected override string GetTitle() => "Dead Zone";
        protected override string GetDescription() => "Even in death, your poison lingers.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Health",
                    amount = "+30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Death Cloud",
                    amount = "10 DPS",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Movement Speed",
                    amount = "-15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
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

    public class DeadZoneEffect : MonoBehaviour
    {
        public Player player;
        private bool wasDead = false;

        private void Update()
        {
            if (player == null) return;

            bool isDead = player.data.dead;

            if (isDead && !wasDead)
            {
                SpawnDeathCloud();
            }

            wasDead = isDead;
        }

        private void SpawnDeathCloud()
        {
            var zoneObj = new GameObject("DeadZone_DeathCloud");
            zoneObj.transform.position = player.transform.position;

            var zone = zoneObj.AddComponent<ZoneBehaviour>();
            zone.radius = 8f;
            zone.duration = 12f;
            zone.damagePerSecond = 10f;
            zone.owner = player;
            zone.affectsOwner = false;
            zone.outerColor = new Color(0.1f, 0.4f, 0.1f, 0.5f);
            zone.innerColor = new Color(0.2f, 0.5f, 0.2f, 0.35f);
        }
    }
}
