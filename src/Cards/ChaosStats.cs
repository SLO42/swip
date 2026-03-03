using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class ChaosStatsStorage : MonoBehaviour
    {
        public float origDamage;
        public float origAttackSpeed;
        public float origMoveSpeed;
        public float origMaxHealth;
    }

    public class ChaosStats : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.AddComponent<ChaosStatsStorage>();
            store.origDamage = gun.damage;
            store.origAttackSpeed = gun.attackSpeed;
            store.origMoveSpeed = characterStats.movementSpeed;
            store.origMaxHealth = data.maxHealth;

            float[] mults = new float[] { 1f, 1f, 1f, 1f };
            int idx1 = Random.Range(0, 4);
            int idx2 = (idx1 + Random.Range(1, 4)) % 4;
            mults[idx1] = Random.Range(0.5f, 2f);
            mults[idx2] = Random.Range(0.5f, 2f);

            gun.damage *= mults[0];
            gun.attackSpeed *= (2f - mults[1]);
            characterStats.movementSpeed *= mults[2];
            data.maxHealth *= mults[3];
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.GetComponent<ChaosStatsStorage>();
            if (store != null)
            {
                gun.damage = store.origDamage;
                gun.attackSpeed = store.origAttackSpeed;
                characterStats.movementSpeed = store.origMoveSpeed;
                data.maxHealth = store.origMaxHealth;
                Object.Destroy(store);
            }
        }

        protected override string GetTitle() => "Chaos Stats";
        protected override string GetDescription() => "Roll the dice on your build.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Effect",
                    amount = "Randomize 2 Stats",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
