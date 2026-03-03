using UnityEngine;

namespace SWIP.Effects
{
    public class FeatherfallBounceEffect : MonoBehaviour
    {
        public float gravityReductionPerBounce = 0.3f;

        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;
        private Rigidbody2D rb;

        private void Start()
        {
            lastPosition = transform.position;
            rb = GetComponent<Rigidbody2D>();
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
                        if (rb != null)
                        {
                            rb.gravityScale = Mathf.Max(0f, rb.gravityScale - gravityReductionPerBounce);
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class FeatherfallBounceSpawner : ProjectileEffectSpawner
    {
        public float gravityReductionPerBounce = 0.3f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<FeatherfallBounceEffect>();
            effect.gravityReductionPerBounce = gravityReductionPerBounce;
        }
    }
}
