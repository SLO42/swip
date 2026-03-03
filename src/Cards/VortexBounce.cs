using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class VortexBounce : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects += 2;

            var s = player.gameObject.AddComponent<VortexBounceSpawner>();
            s.pullRadius = 5f;
            s.pullForce = 800f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.reflects -= 2;

            var s = player.gameObject.GetComponent<VortexBounceSpawner>();
            if (s != null) Object.Destroy(s);
        }

        protected override string GetTitle() => "Vortex Bounce";
        protected override string GetDescription() => "Come closer. CLOSER.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Pull Enemies", amount = "On Bounce", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Bounces", amount = "+2", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
