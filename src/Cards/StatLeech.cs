using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class StatLeech : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var others = PlayerManager.instance.players.FindAll(p => p != player && !p.data.dead);

            foreach (var other in others)
            {
                var otherGun = other.GetComponentInChildren<Gun>();
                if (otherGun == null) continue;

                // Leech 15% of each stat that's higher than yours
                if (otherGun.damage > gun.damage)
                {
                    float leech = otherGun.damage * 0.15f;
                    otherGun.damage -= leech;
                    gun.damage += leech;
                }

                if (other.data.maxHealth > data.maxHealth)
                {
                    float leech = other.data.maxHealth * 0.15f;
                    other.data.maxHealth -= leech;
                    data.maxHealth += leech;
                }
            }

            // Replace self with inert placeholder to prevent re-triggering on swap
            var ph = CardRegistry.Get("Stat Leech Used");
            if (ph != null)
            {
                for (int j = data.currentCards.Count - 1; j >= 0; j--)
                {
                    if (data.currentCards[j].cardName == "Stat Leech")
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

        protected override string GetTitle() => "Stat Leech";
        protected override string GetDescription() => "Siphon the strong. Hope they don't notice.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat { positive = true, stat = "Leech 15%", amount = "Better Stats From Others", simepleAmount = CardInfoStat.SimpleAmount.notAssigned }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.EvilPurple;
        public override string GetModName() => "SWIP";
    }
}
