using UnityEngine;

namespace SWIP.Effects
{
    public class ChaosRicochetEffect : MonoBehaviour
    {
        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;
        private Rigidbody2D rb;
        private ProjectileHit projectileHit;

        private void Start()
        {
            lastPosition = transform.position;
            rb = GetComponent<Rigidbody2D>();
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

                        if (projectileHit != null && rb != null)
                        {
                            float oldDmg = projectileHit.damage;
                            float oldSpeed = rb.velocity.magnitude;
                            projectileHit.damage = oldSpeed;
                            rb.velocity = rb.velocity.normalized * oldDmg;
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class ChaosRicochetSpawner : ProjectileEffectSpawner
    {
        protected override void ApplyToProjectile(GameObject projectile)
        {
            projectile.AddComponent<ChaosRicochetEffect>();
        }
    }
}
