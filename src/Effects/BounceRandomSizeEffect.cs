using UnityEngine;

namespace SWIP.Effects
{
    public class BounceRandomSizeEffect : MonoBehaviour
    {
        public float speedMultPerBounce = 1.15f;

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

                        float scale = Random.Range(0.5f, 2.0f);
                        transform.localScale = Vector3.one * scale;

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

    public class BounceRandomSizeSpawner : ProjectileEffectSpawner
    {
        public float speedMultPerBounce = 1.15f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<BounceRandomSizeEffect>();
            effect.speedMultPerBounce = speedMultPerBounce;
        }
    }
}
