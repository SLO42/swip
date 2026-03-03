using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class HighStakes : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var others = PlayerManager.instance.players.FindAll(p => p != player && !p.data.dead);
            if (others.Count == 0) return;

            var target = others[UnityEngine.Random.Range(0, others.Count)];
            var targetGun = target.GetComponentInChildren<Gun>();

            if (UnityEngine.Random.value > 0.5f)
            {
                // Win: steal 20% of target's damage and health
                float stolenDmg = targetGun.damage * 0.2f;
                float stolenHp = target.data.maxHealth * 0.2f;
                targetGun.damage *= 0.8f;
                target.data.maxHealth *= 0.8f;
                gun.damage += stolenDmg;
                data.maxHealth += stolenHp;
            }
            else
            {
                // Lose: give 20% of your damage and health to target
                float lostDmg = gun.damage * 0.2f;
                float lostHp = data.maxHealth * 0.2f;
                gun.damage *= 0.8f;
                data.maxHealth *= 0.8f;
                targetGun.damage += lostDmg;
                target.data.maxHealth += lostHp;
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("High Stakes Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "High Stakes")
                    {
                        data.currentCards[j] = ph;
                        break;
                    }
                }
            }
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle() => "High Stakes";
        protected override string GetDescription() => "Win big or lose bigger.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "50/50", amount = "Steal or Lose Stats", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
