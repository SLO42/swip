using UnityEngine;
using System.Collections;

namespace SWIP.Effects
{
    public class SmiteOnHitEffect : MonoBehaviour
    {
        public int explosionCount = 3;
        public float explosionDamage = 45f;
        public float explosionRange = 3.5f;
        public float explosionForce = 2000f;
        public float delayBetween = 0.15f;

        void Start()
        {
            var projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                projectileHit.AddHitAction(OnHit);
            }
        }

        private void OnHit()
        {
            var strikeManager = new GameObject("SmiteStrikeManager");
            strikeManager.transform.position = transform.position;
            var runner = strikeManager.AddComponent<SmiteCoroutineRunner>();
            runner.targetPos = transform.position;
            runner.explosionCount = explosionCount;
            runner.explosionDamage = explosionDamage;
            runner.explosionRange = explosionRange;
            runner.explosionForce = explosionForce;
            runner.delayBetween = delayBetween;
        }
    }

    public class SmiteCoroutineRunner : MonoBehaviour
    {
        public Vector3 targetPos;
        public int explosionCount = 3;
        public float explosionDamage = 45f;
        public float explosionRange = 3.5f;
        public float explosionForce = 2000f;
        public float delayBetween = 0.15f;

        void Start()
        {
            StartCoroutine(StrikeCoroutine());
        }

        private IEnumerator StrikeCoroutine()
        {
            for (int i = 0; i < explosionCount; i++)
            {
                Vector3 offset = new Vector3(
                    UnityEngine.Random.Range(-1.5f, 1.5f),
                    UnityEngine.Random.Range(-0.5f, 1f),
                    0f
                );

                var strikeObj = new GameObject("SmiteExplosion");
                strikeObj.transform.position = targetPos + offset;
                var exp = strikeObj.AddComponent<Explosion>();
                exp.auto = true;
                exp.damage = explosionDamage;
                exp.range = explosionRange;
                exp.force = explosionForce;

                yield return new WaitForSeconds(delayBetween);
            }

            Destroy(gameObject);
        }
    }

    public class SmiteOnHitSpawner : ProjectileEffectSpawner
    {
        public int explosionCount = 3;
        public float explosionDamage = 45f;
        public float explosionRange = 3.5f;
        public float explosionForce = 2000f;
        public float delayBetween = 0.15f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<SmiteOnHitEffect>();
            effect.explosionCount = explosionCount;
            effect.explosionDamage = explosionDamage;
            effect.explosionRange = explosionRange;
            effect.explosionForce = explosionForce;
            effect.delayBetween = delayBetween;
        }
    }
}
