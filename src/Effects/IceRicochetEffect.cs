using UnityEngine;

namespace SWIP.Effects
{
    public class IceRicochetEffect : MonoBehaviour
    {
        public float freezeRadius = 4f;
        public float slowPercent = 0.4f;
        public float freezeDuration = 2f;

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

                        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, freezeRadius);
                        foreach (var hit in hits)
                        {
                            Player player = hit.GetComponent<Player>();
                            if (player == null) continue;
                            if (player.data.dead) continue;
                            if (projectileHit != null && projectileHit.ownPlayer != null &&
                                player.teamID == projectileHit.ownPlayer.teamID) continue;

                            var freeze = player.gameObject.AddComponent<FreezeDamageEffect>();
                            freeze.slowPercent = slowPercent;
                            freeze.duration = freezeDuration;
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class IceRicochetSpawner : ProjectileEffectSpawner
    {
        public float freezeRadius = 4f;
        public float slowPercent = 0.4f;
        public float freezeDuration = 2f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<IceRicochetEffect>();
            effect.freezeRadius = freezeRadius;
            effect.slowPercent = slowPercent;
            effect.freezeDuration = freezeDuration;
        }
    }
}
