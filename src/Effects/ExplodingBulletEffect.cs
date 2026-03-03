using UnityEngine;

namespace SWIP.Effects
{
    public class ExplodingBulletEffect : MonoBehaviour
    {
        public float explosionDamage = 55f;
        public float explosionRange = 3f;
        public float explosionForce = 2000f;

        void Start()
        {
            var projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                projectileHit.AddHitAction(Explode);
            }
        }

        private void Explode()
        {
            SWIPExplosion.Spawn(transform.position, explosionDamage, explosionRange, explosionForce);
        }
    }

    public class ExplodingBulletSpawner : ProjectileEffectSpawner
    {
        public float explosionDamage = 55f;
        public float explosionRange = 3f;
        public float explosionForce = 2000f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<ExplodingBulletEffect>();
            effect.explosionDamage = explosionDamage;
            effect.explosionRange = explosionRange;
            effect.explosionForce = explosionForce;
        }
    }
}
