using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class ClipDump : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gunAmmo.maxAmmo += 3;
            gun.damage *= 0.8f;

            player.gameObject.AddComponent<ClipDumpEffect>();
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gunAmmo.maxAmmo -= 3;
            gun.damage /= 0.8f;

            var e = player.gameObject.GetComponent<ClipDumpEffect>();
            if (e != null) Object.Destroy(e);
        }

        protected override string GetTitle() => "Clip Dump";
        protected override string GetDescription() => "Empty the magazine. Ask questions never.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Ammo", amount = "+3", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Clip Dump", amount = "Enabled", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.FirepowerYellow;
        public override string GetModName() => "SWIP";
    }
}
