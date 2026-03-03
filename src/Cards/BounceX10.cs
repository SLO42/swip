using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class BounceX10 : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 10;
            gun.damage *= 0.8f;
            player.gameObject.AddComponent<ScreenBounceSpawner>();
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 10;
            gun.damage /= 0.8f;
            var s = player.gameObject.GetComponent<ScreenBounceSpawner>();
            if (s != null) Object.Destroy(s);
        }

        protected override string GetTitle() => "Bounce x10";
        protected override string GetDescription() => "Your bullet has commitment issues.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Bounces", amount = "+10", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
