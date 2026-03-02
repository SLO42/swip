using UnityEngine;

namespace SWIP.Effects
{
    public class StatOnBounceEffect : MonoBehaviour
    {
        public float damageMultPerBounce = 1.1f;
        public float speedMultPerBounce = 1.1f;

        private ProjectileHit projectileHit;
        private Rigidbody2D rb;
        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            rb = GetComponent<Rigidbody2D>();
            if (rb == null) rb = GetComponentInParent<Rigidbody2D>();
            lastPosition = transform.position;
        }

        void FixedUpdate()
        {
            if (bounceCooldown > 0f)
            {
                bounceCooldown -= Time.fixedDeltaTime;
            }

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

                        if (projectileHit != null)
                        {
                            projectileHit.damage *= damageMultPerBounce;
                        }

                        if (rb != null)
                        {
                            rb.velocity *= speedMultPerBounce;
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class StatOnBounceSpawner : ProjectileEffectSpawner
    {
        public float damageMultPerBounce = 1.1f;
        public float speedMultPerBounce = 1.1f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<StatOnBounceEffect>();
            effect.damageMultPerBounce = damageMultPerBounce;
            effect.speedMultPerBounce = speedMultPerBounce;
        }
    }
}
