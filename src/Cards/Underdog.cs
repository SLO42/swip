using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class UnderdogEffect : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private float baseDamage;

        private void Start()
        {
            player = GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            baseDamage = gun.damage;
        }

        private void LateUpdate()
        {
            if (player == null || player.data == null || gun == null) return;

            float hpRatio = player.data.health / player.data.maxHealth;
            float bonus = (1f - hpRatio) * 1.5f;
            gun.damage = baseDamage * (1f + bonus);
        }

        private void OnDestroy()
        {
            if (gun != null)
            {
                gun.damage = baseDamage;
            }
        }
    }

    public class Underdog : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<UnderdogEffect>();
            data.maxHealth *= 0.85f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var effect = player.gameObject.GetComponent<UnderdogEffect>();
            if (effect != null) Object.Destroy(effect);
            data.maxHealth /= 0.85f;
        }

        protected override string GetTitle() => "Underdog";
        protected override string GetDescription() => "The less you have, the harder you hit.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage (low HP)",
                    amount = "+Up to 150%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "HP",
                    amount = "-15%",
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
