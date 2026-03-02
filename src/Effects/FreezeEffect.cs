using UnityEngine;

namespace SWIP.Effects
{
    public class FreezeDamageEffect : MonoBehaviour
    {
        public float slowPercent = 0.5f;
        public float duration = 3f;

        private float timer;
        private Rigidbody2D rb;
        private LineRenderer iceVisual;
        private float flickerTimer;

        void Start()
        {
            timer = duration;
            var player = GetComponent<Player>();
            if (player != null)
            {
                rb = player.GetComponent<Rigidbody2D>();
            }
            CreateVisual();
        }

        private void CreateVisual()
        {
            var visualObj = new GameObject("FreezeVisual");
            visualObj.transform.SetParent(transform);
            visualObj.transform.localPosition = Vector3.zero;

            iceVisual = visualObj.AddComponent<LineRenderer>();
            iceVisual.useWorldSpace = false;
            iceVisual.startWidth = 0.1f;
            iceVisual.endWidth = 0.1f;
            iceVisual.loop = true;
            iceVisual.material = new Material(Shader.Find("Sprites/Default"));
            iceVisual.startColor = new Color(0.3f, 0.6f, 1f, 0.6f);
            iceVisual.endColor = new Color(0.5f, 0.8f, 1f, 0.6f);

            int segments = 6;
            iceVisual.positionCount = segments;
            UpdateIceShape(1f);
        }

        private void UpdateIceShape(float scale)
        {
            if (iceVisual == null) return;
            int segments = iceVisual.positionCount;
            float baseRadius = 1.2f * scale;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                float r = baseRadius * (1f + Mathf.Sin(angle * 3f + flickerTimer * 2f) * 0.05f);
                iceVisual.SetPosition(i, new Vector3(
                    Mathf.Cos(angle) * r,
                    Mathf.Sin(angle) * r,
                    0f
                ));
            }
        }

        void FixedUpdate()
        {
            if (rb == null)
            {
                CleanupVisual();
                Destroy(this);
                return;
            }

            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                CleanupVisual();
                Destroy(this);
                return;
            }

            flickerTimer += Time.fixedDeltaTime;
            UpdateIceShape(1f);

            if (iceVisual != null)
            {
                float alpha = Mathf.Clamp01(timer / duration) * 0.6f;
                iceVisual.startColor = new Color(0.3f, 0.6f, 1f, alpha);
                iceVisual.endColor = new Color(0.5f, 0.8f, 1f, alpha);
            }

            rb.velocity *= (1f - slowPercent * Time.fixedDeltaTime);
        }

        private void CleanupVisual()
        {
            if (iceVisual != null)
            {
                Destroy(iceVisual.gameObject);
            }
        }

        void OnDestroy()
        {
            CleanupVisual();
        }
    }

    public class FreezeEffect : MonoBehaviour
    {
        public float slowPercent = 0.5f;
        public float freezeDuration = 3f;

        private ProjectileHit projectileHit;
        private Player owner;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                owner = projectileHit.ownPlayer;
                projectileHit.AddHitAction(ApplyFreeze);
            }
        }

        private void ApplyFreeze()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (var hit in hits)
            {
                var player = hit.GetComponentInParent<Player>();
                if (player != null && !player.data.dead && player != owner)
                {
                    var freeze = player.gameObject.AddComponent<FreezeDamageEffect>();
                    freeze.slowPercent = slowPercent;
                    freeze.duration = freezeDuration;
                }
            }
        }
    }

    public class FreezeEffectSpawner : ProjectileEffectSpawner
    {
        public float slowPercent = 0.5f;
        public float freezeDuration = 3f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<FreezeEffect>();
            effect.slowPercent = slowPercent;
            effect.freezeDuration = freezeDuration;
        }
    }
}
