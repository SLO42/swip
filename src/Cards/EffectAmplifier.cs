using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class EffectAmplifier : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) { }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            foreach (var cloud in player.gameObject.GetComponents<CloudEffectSpawner>())
            {
                cloud.cloudRadius *= 1.5f;
                cloud.damagePerSecond *= 1.5f;
                cloud.healPerSecond *= 1.5f;
            }

            foreach (var burn in player.gameObject.GetComponents<ScorchingBounceSpawner>())
            {
                burn.baseBurnDps *= 1.5f;
                burn.burnRadius *= 1.5f;
            }

            foreach (var vortex in player.gameObject.GetComponents<VortexBounceSpawner>())
            {
                vortex.pullRadius *= 1.5f;
                vortex.pullForce *= 1.5f;
            }

            foreach (var shock in player.gameObject.GetComponents<ShockwaveBounceSpawner>())
            {
                shock.pushRadius *= 1.5f;
                shock.pushForce *= 1.5f;
            }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            foreach (var cloud in player.gameObject.GetComponents<CloudEffectSpawner>())
            {
                cloud.cloudRadius /= 1.5f;
                cloud.damagePerSecond /= 1.5f;
                cloud.healPerSecond /= 1.5f;
            }

            foreach (var burn in player.gameObject.GetComponents<ScorchingBounceSpawner>())
            {
                burn.baseBurnDps /= 1.5f;
                burn.burnRadius /= 1.5f;
            }

            foreach (var vortex in player.gameObject.GetComponents<VortexBounceSpawner>())
            {
                vortex.pullRadius /= 1.5f;
                vortex.pullForce /= 1.5f;
            }

            foreach (var shock in player.gameObject.GetComponents<ShockwaveBounceSpawner>())
            {
                shock.pushRadius /= 1.5f;
                shock.pushForce /= 1.5f;
            }
        }

        protected override string GetTitle() => "Effect Amplifier";
        protected override string GetDescription() => "Whatever you're doing, do it LOUDER.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Effect",
                    amount = "All Effect Sizes x1.5",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.MagicPink;
        public override string GetModName() => "SWIP";
    }
}
