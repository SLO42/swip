using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class HotOnesChallenge : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 4f;
            gun.numberOfProjectiles += 6;
            gun.attackSpeed *= 0.5f;
            gun.projectileSpeed *= 2.5f;
            gun.spread *= 3f;
            data.maxHealth *= 0.4f;
            gunAmmo.reloadTime *= 3f;
            gun.ammo -= 3;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage /= 4f;
            gun.numberOfProjectiles -= 6;
            gun.attackSpeed /= 0.5f;
            gun.projectileSpeed /= 2.5f;
            gun.spread /= 3f;
            data.maxHealth /= 0.4f;
            gunAmmo.reloadTime /= 3f;
            gun.ammo += 3;
        }

        protected override string GetTitle() => "Hot Ones Challenge";
        protected override string GetDescription() => "+300% dmg, +6 bullets, everything cranked up.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Damage", amount = "+300%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Bullets", amount = "+6", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Fire Rate", amount = "+100%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Health", amount = "-60%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
