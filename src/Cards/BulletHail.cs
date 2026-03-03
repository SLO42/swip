using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class BulletHail : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles += 10;
            gun.spread += 0.6f;
            gun.damage *= 0.5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles -= 10;
            gun.spread -= 0.6f;
            gun.damage /= 0.5f;
        }

        protected override string GetTitle() => "Bullet Hail";
        protected override string GetDescription() => "Death by a thousand papercuts.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Bullets", amount = "+10", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Spread", amount = "+0.6", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
