using System.Collections.Generic;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class BerserkerEffect : MonoBehaviour
    {
        private Player player;
        private Gun gun;
        private CharacterStatModifiers characterStats;
        private float baseDamage;
        private float baseSpeed;
        private int killCount;
        private Dictionary<Player, bool> previousAliveState = new Dictionary<Player, bool>();
        private bool roundActive;

        private void Start()
        {
            player = GetComponentInParent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            characterStats = player.GetComponent<CharacterStatModifiers>();
            baseDamage = gun.damage;
            baseSpeed = characterStats.movementSpeed;
            killCount = 0;
            roundActive = true;
            InitializeAliveStates();
        }

        private void InitializeAliveStates()
        {
            previousAliveState.Clear();
            foreach (var p in PlayerManager.instance.players.Where(p => p != player))
            {
                previousAliveState[p] = p.data.isPlaying && !p.data.dead;
            }
        }

        private void LateUpdate()
        {
            if (player == null || gun == null || characterStats == null) return;

            // Detect round reset: if player was dead and is now alive, reset
            if (player.data.isPlaying && !player.data.dead && !roundActive)
            {
                roundActive = true;
                killCount = 0;
                InitializeAliveStates();
            }

            if (player.data.dead)
            {
                roundActive = false;
            }

            // Track kills by detecting opponent death transitions
            foreach (var p in PlayerManager.instance.players.Where(p => p != player))
            {
                bool currentlyAlive = p.data.isPlaying && !p.data.dead;

                if (previousAliveState.ContainsKey(p))
                {
                    if (previousAliveState[p] && !currentlyAlive)
                    {
                        killCount++;
                    }
                }

                previousAliveState[p] = currentlyAlive;
            }

            gun.damage = baseDamage * (1f + killCount * 0.15f);
            characterStats.movementSpeed = baseSpeed * (1f + killCount * 0.1f);
        }

        private void OnDestroy()
        {
            if (gun != null)
            {
                gun.damage = baseDamage;
            }
            if (characterStats != null)
            {
                characterStats.movementSpeed = baseSpeed;
            }
        }
    }

    public class Berserker : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<BerserkerEffect>();
            data.maxHealth *= 0.85f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var effect = player.gameObject.GetComponent<BerserkerEffect>();
            if (effect != null) Object.Destroy(effect);
            data.maxHealth /= 0.85f;
        }

        protected override string GetTitle() => "Berserker";
        protected override string GetDescription() => "Each kill makes you stronger. Much stronger.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage/kill",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Speed/kill",
                    amount = "+10%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "HP",
                    amount = "-15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityLib.Utils.RarityUtils.GetRarity("Epic");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";
    }
}
