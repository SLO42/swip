using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class LaserPrecision : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.6f;
            gun.attackSpeed *= 0.3f;
            gun.ammo += 10;
            gunAmmo.reloadTime = 0f;
            gun.projectileSpeed *= 5f;
            gun.gravity = 0f;
            gun.spread = 0f;

            var laser = player.gameObject.AddComponent<LaserBeamEffect>();
            laser.laserDamage = 30f;
            laser.laserRange = 120f;
            laser.beamWidth = 0.4f;
            laser.beamDuration = 0.25f;
            laser.beamColor = new Color(1f, 0.1f, 0.05f, 0.95f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 0.6f;
            gun.attackSpeed /= 0.3f;
            gun.ammo -= 10;
            gun.projectileSpeed /= 5f;

            var laser = player.gameObject.GetComponent<LaserBeamEffect>();
            if (laser != null) Object.Destroy(laser);
        }

        protected override string GetTitle() => "Laser Precision";
        protected override string GetDescription() => "Pew pew pew pew pew pew pew.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Laser Beam", amount = "On Shoot", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Ammo", amount = "+10", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Fire Rate", amount = "+230%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "No Reload", amount = "Yes", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
