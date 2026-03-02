using UnityEngine;

namespace SWIP.Effects
{
    public class GrowOnBounceEffect : MonoBehaviour
    {
        public float growthFactor = 1.5f;

        private ProjectileHit projectileHit;
        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
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
                        transform.localScale *= growthFactor;

                        if (projectileHit != null)
                        {
                            projectileHit.damage *= growthFactor;
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class GrowOnBounceSpawner : ProjectileEffectSpawner
    {
        public float growthFactor = 1.5f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<GrowOnBounceEffect>();
            effect.growthFactor = growthFactor;
        }
    }
}
