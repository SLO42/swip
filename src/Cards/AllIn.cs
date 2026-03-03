using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class AllInStorage : MonoBehaviour
    {
        public float origDamage;
        public float origAttackSpeed;
        public float origMoveSpeed;
        public float origMaxHealth;
        public float origProjSpeed;
    }

    public class AllIn : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.AddComponent<AllInStorage>();
            store.origDamage = gun.damage;
            store.origAttackSpeed = gun.attackSpeed;
            store.origMoveSpeed = characterStats.movementSpeed;
            store.origMaxHealth = data.maxHealth;
            store.origProjSpeed = gun.projectileSpeed;

            gun.damage *= Random.Range(0.5f, 1.5f);
            gun.attackSpeed *= Random.Range(0.5f, 1.5f);
            characterStats.movementSpeed *= Random.Range(0.5f, 1.5f);
            data.maxHealth *= Random.Range(0.5f, 1.5f);
            gun.projectileSpeed *= Random.Range(0.5f, 1.5f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.GetComponent<AllInStorage>();
            if (store != null)
            {
                gun.damage = store.origDamage;
                gun.attackSpeed = store.origAttackSpeed;
                characterStats.movementSpeed = store.origMoveSpeed;
                data.maxHealth = store.origMaxHealth;
                gun.projectileSpeed = store.origProjSpeed;
                Object.Destroy(store);
            }
        }

        protected override string GetTitle() => "All In";
        protected override string GetDescription() => "Everything changes. No refunds.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Effect",
                    amount = "Randomize All Stats +/-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
