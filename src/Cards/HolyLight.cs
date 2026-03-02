using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class HolyLight : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.regen += 20f;
            gun.damage *= 0.85f;
            gun.gravity = 0f;
            gun.projectileSpeed *= 4f;
            gunAmmo.reloadTime = 0f;

            var laser = player.gameObject.AddComponent<LaserBeamEffect>();
            laser.laserDamage = 40f;
            laser.laserRange = 80f;
            laser.beamWidth = 0.4f;
            laser.beamDuration = 0.25f;
            laser.beamColor = new Color(0.4f, 0.7f, 1f, 0.9f);
            laser.beamCoreColor = new Color(0.8f, 0.95f, 1f, 1f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.regen -= 20f;
            gun.damage /= 0.85f;
            gun.projectileSpeed /= 4f;

            var laser = player.gameObject.GetComponent<LaserBeamEffect>();
            if (laser != null) Object.Destroy(laser);
        }

        protected override string GetTitle() => "Holy Light";
        protected override string GetDescription() => "Heals you. Burns them.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Laser Beam", amount = "On Shoot", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Regen", amount = "+20", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "No Reload", amount = "Yes", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Damage", amount = "-15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DefensiveBlue;
        public override string GetModName() => "SWIP";
    }
}
