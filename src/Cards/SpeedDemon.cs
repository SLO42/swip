using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class SpeedDemonStorage : MonoBehaviour
    {
        public float origMoveSpeed;
        public float origDamage;
    }

    public class SpeedDemon : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.AddComponent<SpeedDemonStorage>();
            store.origMoveSpeed = characterStats.movementSpeed;
            store.origDamage = gun.damage;

            gun.damage *= (1f + characterStats.movementSpeed * 0.5f);
            characterStats.movementSpeed *= 0.5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.GetComponent<SpeedDemonStorage>();
            if (store != null)
            {
                gun.damage = store.origDamage;
                characterStats.movementSpeed = store.origMoveSpeed;
                Object.Destroy(store);
            }
        }

        protected override string GetTitle() => "Speed Demon";
        protected override string GetDescription() => "Trade agility for firepower. Or is it the other way around?";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Effect",
                    amount = "Swap Speed for Damage",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
