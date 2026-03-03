using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class RisingTideStorage : MonoBehaviour
    {
        public float dmgMult;
        public float spdMult;
        public float movMult;
        public float hpMult;
    }

    public class RisingTide : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.AddComponent<RisingTideStorage>();
            store.dmgMult = 1f;
            store.spdMult = 1f;
            store.movMult = 1f;
            store.hpMult = 1f;

            if (gun.damage > 1f) { store.dmgMult = 1.2f; gun.damage *= 1.2f; }
            if (gun.attackSpeed < 1f) { store.spdMult = 0.8f; gun.attackSpeed *= 0.8f; }
            if (characterStats.movementSpeed > 1f) { store.movMult = 1.2f; characterStats.movementSpeed *= 1.2f; }
            if (data.maxHealth > 100f) { store.hpMult = 1.2f; data.maxHealth *= 1.2f; }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var store = player.gameObject.GetComponent<RisingTideStorage>();
            if (store != null)
            {
                gun.damage /= store.dmgMult;
                gun.attackSpeed /= store.spdMult;
                characterStats.movementSpeed /= store.movMult;
                data.maxHealth /= store.hpMult;
                Object.Destroy(store);
            }
        }

        protected override string GetTitle() => "Rising Tide";
        protected override string GetDescription() => "The rich get richer.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Effect",
                    amount = "+20% to Above-Average Stats",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.NatureBrown;
        public override string GetModName() => "SWIP";
    }
}
