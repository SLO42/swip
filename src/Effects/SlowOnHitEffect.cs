using UnityEngine;

namespace SWIP.Effects
{
    public class SlowDebuffEffect : MonoBehaviour
    {
        public float slowPercent = 0.3f;
        public float duration = 2f;

        private float timer;
        private Rigidbody2D rb;
        private LineRenderer visual;
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
            var visualObj = new GameObject("SlowVisual");
            visualObj.transform.SetParent(transform);
            visualObj.transform.localPosition = Vector3.zero;

            visual = visualObj.AddComponent<LineRenderer>();
            visual.useWorldSpace = false;
            visual.startWidth = 0.1f;
            visual.endWidth = 0.1f;
            visual.loop = true;
            visual.material = new Material(Shader.Find("Sprites/Default"));
            visual.startColor = new Color(0.4f, 1f, 0.6f, 0.6f);
            visual.endColor = new Color(0.3f, 0.9f, 0.5f, 0.6f);

            int segments = 8;
            visual.positionCount = segments;
            UpdateShape(1f);
        }

        private void UpdateShape(float scale)
        {
            if (visual == null) return;
            int segments = visual.positionCount;
            float baseRadius = 1.2f * scale;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                float r = baseRadius * (1f + Mathf.Sin(angle * 4f + flickerTimer * 3f) * 0.05f);
                visual.SetPosition(i, new Vector3(
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
            UpdateShape(1f);

            if (visual != null)
            {
                float alpha = Mathf.Clamp01(timer / duration) * 0.6f;
                visual.startColor = new Color(0.4f, 1f, 0.6f, alpha);
                visual.endColor = new Color(0.3f, 0.9f, 0.5f, alpha);
            }

            rb.velocity *= (1f - slowPercent * Time.fixedDeltaTime);
        }

        private void CleanupVisual()
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
            }
        }

        void OnDestroy()
        {
            CleanupVisual();
        }
    }

    public class SlowOnHitEffect : MonoBehaviour
    {
        public float slowPercent = 0.3f;
        public float slowDuration = 2f;

        private ProjectileHit projectileHit;
        private Player owner;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                owner = projectileHit.ownPlayer;
                projectileHit.AddHitAction(ApplySlow);
            }
        }

        private void ApplySlow()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (var hit in hits)
            {
                var player = hit.GetComponentInParent<Player>();
                if (player != null && !player.data.dead && player != owner)
                {
                    var slow = player.gameObject.AddComponent<SlowDebuffEffect>();
                    slow.slowPercent = slowPercent;
                    slow.duration = slowDuration;
                }
            }
        }
    }

    public class SlowOnHitSpawner : ProjectileEffectSpawner
    {
        public float slowPercent = 0.3f;
        public float slowDuration = 2f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<SlowOnHitEffect>();
            effect.slowPercent = slowPercent;
            effect.slowDuration = slowDuration;
        }
    }
}
