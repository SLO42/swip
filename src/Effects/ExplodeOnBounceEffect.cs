using UnityEngine;

namespace SWIP.Effects
{
    public class ExplodeOnBounceEffect : MonoBehaviour
    {
        public float explosionDamage = 20f;
        public float explosionRange = 2f;
        public float explosionForce = 800f;

        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;

        void Start()
        {
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
                    if (dot < 0.5f)
                    {
                        bounceCooldown = 0.15f;

                        var expObj = new GameObject("BounceExplosion");
                        expObj.transform.position = transform.position;
                        var exp = expObj.AddComponent<Explosion>();
                        exp.auto = true;
                        exp.damage = explosionDamage;
                        exp.range = explosionRange;
                        exp.force = explosionForce;
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class ExplodeOnBounceSpawner : ProjectileEffectSpawner
    {
        public float explosionDamage = 20f;
        public float explosionRange = 2f;
        public float explosionForce = 800f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<ExplodeOnBounceEffect>();
            effect.explosionDamage = explosionDamage;
            effect.explosionRange = explosionRange;
            effect.explosionForce = explosionForce;
        }
    }
}
