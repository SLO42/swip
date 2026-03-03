using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class DoubleDownStorage : MonoBehaviour
    {
        public float dmgMult;
        public float spdMult;
        public float movMult;
        public float hpMult;
    }

    public class DoubleDown : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.AddComponent<DoubleDownStorage>();
            store.dmgMult = 1f;
            store.spdMult = 1f;
            store.movMult = 1f;
            store.hpMult = 1f;

            if (gun.damage < 1f) { float m = Random.value > 0.5f ? 2f : 0.5f; store.dmgMult = m; gun.damage *= m; }
            if (gun.attackSpeed > 1f) { float m = Random.value > 0.5f ? 0.5f : 2f; store.spdMult = m; gun.attackSpeed *= m; }
            if (characterStats.movementSpeed < 1f) { float m = Random.value > 0.5f ? 2f : 0.5f; store.movMult = m; characterStats.movementSpeed *= m; }
            if (data.maxHealth < 100f) { float m = Random.value > 0.5f ? 2f : 0.5f; store.hpMult = m; data.maxHealth *= m; }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.GetComponent<DoubleDownStorage>();
            if (store != null)
            {
                gun.damage /= store.dmgMult;
                gun.attackSpeed /= store.spdMult;
                characterStats.movementSpeed /= store.movMult;
                data.maxHealth /= store.hpMult;
                Object.Destroy(store);
            }
        }

        protected override string GetTitle() => "Double Down";
        protected override string GetDescription() => "Feeling lucky, punk?";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Effect",
                    amount = "50/50 Double/Halve Weak Stats",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
