using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class LastStandEffect : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private CharacterStatModifiers characterStats;
        private float baseDamage;
        private float baseSpeed;

        private void Start()
        {
            player = GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            characterStats = player.GetComponent<CharacterStatModifiers>();
            baseDamage = gun.damage;
            baseSpeed = characterStats.movementSpeed;
        }

        private void LateUpdate()
        {
            if (player == null || player.data == null || gun == null || characterStats == null) return;

            float hpRatio = player.data.health / player.data.maxHealth;

            if (hpRatio < 0.25f)
            {
                gun.damage = baseDamage * 2f;
                characterStats.movementSpeed = baseSpeed * 1.5f;
            }
            else
            {
                gun.damage = baseDamage;
                characterStats.movementSpeed = baseSpeed;
            }
        }

        private void OnDestroy()
        {
            if (gun != null)
            {
                gun.damage = baseDamage;
            }
            if (characterStats != null)
            {
                characterStats.movementSpeed = baseSpeed;
            }
        }
    }

    public class LastStand : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<LastStandEffect>();
            data.maxHealth *= 0.9f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var effect = player.gameObject.GetComponent<LastStandEffect>();
            if (effect != null) Object.Destroy(effect);
            data.maxHealth /= 0.9f;
        }

        protected override string GetTitle() => "Last Stand";
        protected override string GetDescription() => "When all seems lost, you become unstoppable.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage (below 25% HP)",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Speed (below 25% HP)",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "HP",
                    amount = "-10%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityLib.Utils.RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
