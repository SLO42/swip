using UnityEngine;

namespace SWIP.Effects
{
    public class CloudEffect : MonoBehaviour
    {
        public float cloudRadius = 3f;
        public float cloudDuration = 4f;
        public float damagePerSecond;
        public float healPerSecond;
        public float slowAmount;
        public Color outerColor = new Color(0.2f, 0.8f, 0.2f, 0.5f);
        public Color innerColor = new Color(0.3f, 0.9f, 0.3f, 0.35f);
        public bool affectsOwner;
        public bool spawnOnBounce;
        public bool scaleWithDamage;

        private ProjectileHit projectileHit;
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
                projectileHit.AddHitAction(SpawnCloud);
            }

            lastPosition = transform.position;
        }

        void FixedUpdate()
        {
            if (!spawnOnBounce) return;

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
                        SpawnCloud();
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }

        private void SpawnCloud()
        {
            var cloudObj = new GameObject("Zone");
            cloudObj.transform.position = transform.position;
            var zone = cloudObj.AddComponent<ZoneBehaviour>();
            zone.radius = cloudRadius;
            zone.duration = cloudDuration;
            zone.outerColor = outerColor;
            zone.innerColor = innerColor;
            zone.affectsOwner = affectsOwner;

            float scale = 1f;
            if (scaleWithDamage && projectileHit != null)
            {
                scale = projectileHit.damage / 55f;
            }

            zone.damagePerSecond = damagePerSecond * scale;
            zone.healPerSecond = healPerSecond * scale;
            zone.slowAmount = slowAmount;
        }
    }

    public class CloudEffectSpawner : ProjectileEffectSpawner
    {
        public float cloudRadius = 3f;
        public float cloudDuration = 4f;
        public float damagePerSecond;
        public float healPerSecond;
        public float slowAmount;
        public Color outerColor = new Color(0.2f, 0.8f, 0.2f, 0.5f);
        public Color innerColor = new Color(0.3f, 0.9f, 0.3f, 0.35f);
        public bool affectsOwner;
        public bool spawnOnBounce;
        public bool scaleWithDamage;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<CloudEffect>();
            effect.cloudRadius = cloudRadius;
            effect.cloudDuration = cloudDuration;
            effect.damagePerSecond = damagePerSecond;
            effect.healPerSecond = healPerSecond;
            effect.slowAmount = slowAmount;
            effect.outerColor = outerColor;
            effect.innerColor = innerColor;
            effect.affectsOwner = affectsOwner;
            effect.spawnOnBounce = spawnOnBounce;
            effect.scaleWithDamage = scaleWithDamage;
        }
    }
}
