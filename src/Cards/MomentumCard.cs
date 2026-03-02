using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class MomentumEffect : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private CharacterStatModifiers characterStats;
        private float baseDamage;

        private void Start()
        {
            player = GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            characterStats = player.GetComponent<CharacterStatModifiers>();
            baseDamage = gun.damage;
        }

        private void LateUpdate()
        {
            if (player == null || gun == null || characterStats == null) return;

            float speedRatio = characterStats.movementSpeed;
            float bonus = (speedRatio - 1f) * 0.8f;
            if (bonus > 0)
            {
                gun.damage = baseDamage * (1f + bonus);
            }
            else
            {
                gun.damage = baseDamage;
            }
        }

        private void OnDestroy()
        {
            if (gun != null)
            {
                gun.damage = baseDamage;
            }
        }
    }

    public class MomentumCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<MomentumEffect>();
            characterStats.movementSpeed *= 1.15f;
            gun.damage *= 0.9f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var effect = player.gameObject.GetComponent<MomentumEffect>();
            if (effect != null) Object.Destroy(effect);
            characterStats.movementSpeed /= 1.15f;
            gun.damage /= 0.9f;
        }

        protected override string GetTitle() => "Momentum";
        protected override string GetDescription() => "Speed kills. Literally.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Speed",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-10%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Speed scales Damage",
                    amount = "+",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityLib.Utils.RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
