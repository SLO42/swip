using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class Napalm : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.projectileSize *= 1.5f;
            gun.attackSpeed *= 2f;
            data.maxHealth *= 0.75f;

            // Burn effect
            var burnSpawner = player.gameObject.AddComponent<BurnEffectSpawner>();
            burnSpawner.burnDPS = 10f;
            burnSpawner.burnDuration = 4f;

            // Fire cloud effect
            var cloudSpawner = player.gameObject.AddComponent<CloudEffectSpawner>();
            cloudSpawner.cloudRadius = 3f;
            cloudSpawner.cloudDuration = 4f;
            cloudSpawner.damagePerSecond = 10f;
            cloudSpawner.outerColor = new Color(1f, 0.5f, 0f, 0.5f);
            cloudSpawner.innerColor = new Color(1f, 0.7f, 0.3f, 0.35f);
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.projectileSize /= 1.5f;
            gun.attackSpeed /= 2f;
            data.maxHealth /= 0.75f;

            var burnSpawner = player.gameObject.GetComponent<BurnEffectSpawner>();
            if (burnSpawner != null) Object.Destroy(burnSpawner);
            var cloudSpawner = player.gameObject.GetComponent<CloudEffectSpawner>();
            if (cloudSpawner != null) Object.Destroy(cloudSpawner);
        }

        protected override string GetTitle() => "Napalm";
        protected override string GetDescription() => "Fire. Everywhere. Forever.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Burn", amount = "10dps x 4s", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Fire Cloud", amount = "10dps, R3, 4s", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Bullet Size", amount = "+50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Fire Rate", amount = "-50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
