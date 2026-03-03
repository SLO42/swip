using UnityEngine;

namespace SWIP.Effects
{
    public class ScorchingBounceEffect : MonoBehaviour
    {
        public float burnRadius = 4f;
        public float baseBurnDps = 5f;
        public float burnDuration = 2f;
        public float escalation = 1.5f;

        private int bounceCount;
        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;
        private ProjectileHit projectileHit;

        private void Start()
        {
            lastPosition = transform.position;
            projectileHit = GetComponent<ProjectileHit>();
        }

        private void FixedUpdate()
        {
            if (bounceCooldown > 0f) bounceCooldown -= Time.fixedDeltaTime;

            Vector3 currentPos = transform.position;
            Vector3 delta = currentPos - lastPosition;

            if (delta.sqrMagnitude > 0.01f)
            {
                Vector3 currentDir = delta.normalized;

                if (hasDirection && bounceCooldown <= 0f)
                {
                    float dot = Vector3.Dot(lastDirection, currentDir);
                    if (dot < 0.7f)
                    {
                        bounceCooldown = 0.15f;
                        bounceCount++;

                        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, burnRadius);
                        foreach (var hit in hits)
                        {
                            Player player = hit.GetComponent<Player>();
                            if (player == null) continue;
                            if (player.data.dead) continue;
                            if (projectileHit != null && projectileHit.ownPlayer != null &&
                                player.teamID == projectileHit.ownPlayer.teamID) continue;

                            var burn = player.gameObject.AddComponent<BurnDamageEffect>();
                            burn.damagePerSecond = baseBurnDps * Mathf.Pow(escalation, bounceCount - 1);
                            burn.duration = burnDuration;
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class ScorchingBounceSpawner : ProjectileEffectSpawner
    {
        public float burnRadius = 4f;
        public float baseBurnDps = 5f;
        public float burnDuration = 2f;
        public float escalation = 1.5f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<ScorchingBounceEffect>();
            effect.burnRadius = burnRadius;
            effect.baseBurnDps = baseBurnDps;
            effect.burnDuration = burnDuration;
            effect.escalation = escalation;
        }
    }
}
