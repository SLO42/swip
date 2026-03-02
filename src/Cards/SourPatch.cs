using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class SourPatch : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.slow += 0.5f;
            gun.damage *= 1.3f;
            gun.projectileSpeed *= 0.75f;
            characterStats.movementSpeed *= 0.85f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.slow -= 0.5f;
            gun.damage /= 1.3f;
            gun.projectileSpeed /= 0.75f;
            characterStats.movementSpeed /= 0.85f;
        }

        protected override string GetTitle() => "Sour Patch";
        protected override string GetDescription() => "Slow and painful. Just like ASourFruit's comebacks.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Slow on Hit", amount = "+50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Damage", amount = "+30%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Bullet Speed", amount = "-25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Speed", amount = "-15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
