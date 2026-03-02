using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class DivineJudgment : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 1.6f;
            gun.attackSpeed *= 2f;
            gun.ammo -= 1;
            gun.reflects += 2;
            gun.gravity = 0f;
            gun.projectileSpeed *= 4f;
            gunAmmo.reloadTime = 0f;

            var laser = player.gameObject.AddComponent<LaserBeamEffect>();
            laser.laserDamage = 70f;
            laser.laserRange = 150f;
            laser.beamWidth = 0.6f;
            laser.beamDuration = 0.35f;
            laser.beamColor = new Color(1f, 0.85f, 0.2f, 0.95f);
            laser.beamCoreColor = new Color(1f, 1f, 0.8f, 1f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 1.6f;
            gun.attackSpeed /= 2f;
            gun.ammo += 1;
            gun.reflects -= 2;
            gun.projectileSpeed /= 4f;

            var laser = player.gameObject.GetComponent<LaserBeamEffect>();
            if (laser != null) Object.Destroy(laser);
        }

        protected override string GetTitle() => "Divine Judgment";
        protected override string GetDescription() => "From the heavens above.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Holy Laser", amount = "On Shoot", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Damage", amount = "+60%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Bounces", amount = "+2", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Fire Rate", amount = "-50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
