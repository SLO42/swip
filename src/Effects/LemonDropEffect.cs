using UnityEngine;

namespace SWIP.Effects
{
    public class LemonDropEffect : MonoBehaviour
    {
        public float blindRadius = 3f;
        public float blindDuration = 2f;
        public float slowAmount = 0.6f;

        private ProjectileHit projectileHit;
        private Player owner;
        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                owner = projectileHit.ownPlayer;
                projectileHit.AddHitAction(SpawnLemon);
            }
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
                        SpawnLemon();
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }

        private void SpawnLemon()
        {
            var zoneObj = new GameObject("LemonZone");
            zoneObj.transform.position = transform.position;
            var zone = zoneObj.AddComponent<ZoneBehaviour>();
            zone.radius = blindRadius;
            zone.duration = blindDuration;
            zone.slowAmount = slowAmount;
            zone.damagePerSecond = 0f;
            zone.outerColor = new Color(1f, 1f, 0.2f, 0.5f);
            zone.innerColor = new Color(1f, 0.95f, 0.4f, 0.35f);
            zone.owner = owner;
        }
    }

    public class LemonDropSpawner : ProjectileEffectSpawner
    {
        public float blindRadius = 3f;
        public float blindDuration = 2f;
        public float slowAmount = 0.6f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<LemonDropEffect>();
            effect.blindRadius = blindRadius;
            effect.blindDuration = blindDuration;
            effect.slowAmount = slowAmount;
        }
    }
}
