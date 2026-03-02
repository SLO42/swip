using System.Collections.Generic;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;
using RarityLib.Utils;

namespace SWIP.Cards
{
    public class Supernova : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 1.5f;
            gun.damage *= 0.7f;
            characterStats.movementSpeed *= 0.8f;

            player.gameObject.AddComponent<SupernovaEffect>();
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 1.5f;
            gun.damage /= 0.7f;
            characterStats.movementSpeed /= 0.8f;

            var effect = player.gameObject.GetComponent<SupernovaEffect>();
            if (effect != null) Object.Destroy(effect);
        }

        protected override string GetTitle() => "Supernova";
        protected override string GetDescription() => "In death, you become a star. A very angry star.";
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Health",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Movement Speed",
                    amount = "-20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Death Explosion",
                    amount = "200 DMG, Burn",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardInfo.Rarity GetRarity() => RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.DestructiveRed;
        public override string GetModName() => "SWIP";

        public class SupernovaEffect : MonoBehaviour
        {
            private Player player;
            private bool wasDead;

            private void Start()
            {
                player = GetComponent<Player>();
                wasDead = false;
            }

            private void Update()
            {
                if (player == null || player.data == null) return;

                bool isDead = player.data.dead;

                if (isDead && !wasDead)
                {
                    Explode();
                }

                wasDead = isDead;
            }

            private void Explode()
            {
                Vector3 position = player.transform.position;
                float explosionRange = 10f;
                float explosionDamage = 200f;
                float explosionForce = 5000f;

                // Create explosion
                var explosionObj = new GameObject("SupernovaExplosion");
                explosionObj.transform.position = position;
                var explosion = explosionObj.AddComponent<Explosion>();
                explosion.damage = explosionDamage;
                explosion.range = explosionRange;
                explosion.force = explosionForce;
                explosion.auto = true;

                // Apply burn to all players in radius
                List<Player> allPlayers = PlayerManager.instance.players;
                foreach (var target in allPlayers.Where(p => p.playerID != player.playerID && !p.data.dead))
                {
                    float distance = Vector3.Distance(position, target.transform.position);
                    if (distance <= explosionRange)
                    {
                        var burnDamage = target.gameObject.AddComponent<BurnDamageEffect>();
                        burnDamage.burnDPS = 20f;
                        burnDamage.burnDuration = 8f;
                    }
                }
            }
        }

        public class BurnDamageEffect : MonoBehaviour
        {
            public float burnDPS = 20f;
            public float burnDuration = 8f;
            private float elapsed;
            private Player target;
            private float tickTimer;
            private const float TICK_INTERVAL = 0.25f;

            private void Start()
            {
                target = GetComponent<Player>();
                elapsed = 0f;
                tickTimer = 0f;
            }

            private void Update()
            {
                if (target == null || target.data.dead)
                {
                    Destroy(this);
                    return;
                }

                elapsed += Time.deltaTime;
                tickTimer += Time.deltaTime;

                if (tickTimer >= TICK_INTERVAL)
                {
                    float damage = burnDPS * tickTimer;
                    target.data.healthHandler.TakeDamage(Vector2.up * damage, target.transform.position);
                    tickTimer = 0f;
                }

                if (elapsed >= burnDuration)
                {
                    Destroy(this);
                }
            }
        }
    }
}
