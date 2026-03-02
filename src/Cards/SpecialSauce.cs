using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class SpecialSauce : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 1.15f;
            characterStats.movementSpeed *= 1.1f;
            gun.damage *= 1.15f;
            gun.attackSpeed *= 0.85f;
            gun.projectileSpeed *= 1.15f;
            gun.ammo += 1;
            gun.reflects += 1;
            gun.spread *= 0.9f;
            gunAmmo.reloadTime *= 0.9f;
            block.cdMultiplier *= 0.9f;
            gravity.gravityForce *= 0.9f;
            characterStats.regen += 2f;
            characterStats.lifeSteal += 0.1f;
            characterStats.sizeMultiplier *= 0.9f;
            characterStats.secondsToTakeDamageOver += 1f;
            gun.numberOfProjectiles += 1;
            gun.projectileSize *= 1.1f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 1.15f;
            characterStats.movementSpeed /= 1.1f;
            gun.damage /= 1.15f;
            gun.attackSpeed /= 0.85f;
            gun.projectileSpeed /= 1.15f;
            gun.ammo -= 1;
            gun.reflects -= 1;
            gun.spread /= 0.9f;
            gunAmmo.reloadTime /= 0.9f;
            block.cdMultiplier /= 0.9f;
            gravity.gravityForce /= 0.9f;
            characterStats.regen -= 2f;
            characterStats.lifeSteal -= 0.1f;
            characterStats.sizeMultiplier /= 0.9f;
            characterStats.secondsToTakeDamageOver -= 1f;
            gun.numberOfProjectiles -= 1;
            gun.projectileSize /= 1.1f;
        }

        protected override string GetTitle() => "Special Sauce";
        protected override string GetDescription() => "A little bit of everything. SaucyEnchiladas' signature.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Everything", amount = "+10-20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Regen", amount = "+2", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Life Steal", amount = "+10%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
