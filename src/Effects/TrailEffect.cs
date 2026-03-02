using UnityEngine;

namespace SWIP.Effects
{
    public class TrailEffect : MonoBehaviour
    {
        public float zoneRadius = 1f;
        public float zoneDuration = 2f;
        public float damagePerSecond;
        public float healPerSecond;
        public float slowAmount;
        public Color outerColor = new Color(1f, 0.5f, 0f, 0.5f);
        public Color innerColor = new Color(1f, 0.7f, 0.3f, 0.35f);
        public bool affectsOwner;
        public float dropInterval = 0.15f;

        private float dropTimer;
        private MoveTransform moveTransform;
        private Vector3 lastDropPos;
        private bool hasLastPos;

        void Start()
        {
            moveTransform = GetComponentInParent<MoveTransform>();
            if (moveTransform == null) moveTransform = GetComponent<MoveTransform>();
            dropTimer = 0f;
        }

        void FixedUpdate()
        {
            if (moveTransform == null) return;

            dropTimer -= Time.fixedDeltaTime;
            if (dropTimer <= 0f)
            {
                dropTimer = dropInterval;

                // Fill gaps: if bullet moved far since last drop, spawn zones along the path
                if (hasLastPos)
                {
                    Vector3 current = transform.position;
                    float dist = Vector3.Distance(lastDropPos, current);
                    float spacing = zoneRadius * 1.2f;

                    if (dist > spacing * 2f)
                    {
                        // Spawn intermediate zones to fill the gap
                        int fills = Mathf.FloorToInt(dist / spacing);
                        Vector3 dir = (current - lastDropPos).normalized;
                        for (int i = 1; i < fills; i++)
                        {
                            SpawnZoneAt(lastDropPos + dir * (spacing * i));
                        }
                    }
                }

                SpawnZoneAt(transform.position);
                lastDropPos = transform.position;
                hasLastPos = true;
            }
        }

        private void SpawnZoneAt(Vector3 pos)
        {
            var zoneObj = new GameObject("TrailZone");
            zoneObj.transform.position = pos;
            var zone = zoneObj.AddComponent<ZoneBehaviour>();
            zone.radius = zoneRadius;
            zone.duration = zoneDuration;
            zone.damagePerSecond = damagePerSecond;
            zone.healPerSecond = healPerSecond;
            zone.slowAmount = slowAmount;
            zone.outerColor = outerColor;
            zone.innerColor = innerColor;
            zone.affectsOwner = affectsOwner;
        }
    }

    public class TrailEffectSpawner : ProjectileEffectSpawner
    {
        public float zoneRadius = 1f;
        public float zoneDuration = 2f;
        public float damagePerSecond;
        public float healPerSecond;
        public float slowAmount;
        public Color outerColor = new Color(1f, 0.5f, 0f, 0.5f);
        public Color innerColor = new Color(1f, 0.7f, 0.3f, 0.35f);
        public bool affectsOwner;
        public float dropInterval = 0.15f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<TrailEffect>();
            effect.zoneRadius = zoneRadius;
            effect.zoneDuration = zoneDuration;
            effect.damagePerSecond = damagePerSecond;
            effect.healPerSecond = healPerSecond;
            effect.slowAmount = slowAmount;
            effect.outerColor = outerColor;
            effect.innerColor = innerColor;
            effect.affectsOwner = affectsOwner;
            effect.dropInterval = dropInterval;
        }
    }
}
