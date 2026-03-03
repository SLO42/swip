using UnboundLib.Cards;
using UnityEngine;
using SWIP.Effects;

namespace SWIP.Cards
{
    public class SynapseFire : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.spread *= 1.8f;
            gun.numberOfProjectiles += 2;
            gun.attackSpeed *= 0.82f;

            var wave = player.gameObject.AddComponent<WaveMotionSpawner>();
            wave.waveAmplitude = 5f;
            wave.waveFrequency = 4f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.spread /= 1.8f;
            gun.numberOfProjectiles -= 2;
            gun.attackSpeed /= 0.82f;

            var wave = player.gameObject.GetComponent<WaveMotionSpawner>();
            if (wave != null) Object.Destroy(wave);
        }

        protected override string GetTitle() => "Synapse Fire";
        protected override string GetDescription() => "Neurons firing in every direction.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Wave Bullets", amount = "Yes", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = false, stat = "Spread", amount = "+80%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Bullets", amount = "+2", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
                new CardInfoStat { positive = true, stat = "Fire Rate", amount = "+18%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
