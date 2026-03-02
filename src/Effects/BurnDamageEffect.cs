using UnityEngine;

namespace SWIP.Effects
{
    public class BurnDamageEffect : MonoBehaviour
    {
        public float damagePerSecond = 5f;
        public float duration = 3f;

        private float timer;
        private HealthHandler healthHandler;
        private LineRenderer fireVisual;
        private float flickerTimer;

        void Start()
        {
            timer = duration;
            var player = GetComponent<Player>();
            if (player != null)
            {
                healthHandler = player.data.healthHandler;
            }
            CreateVisual();
        }

        private void CreateVisual()
        {
            // Create a flickering fire-colored ring around the burning player
            var visualObj = new GameObject("BurnVisual");
            visualObj.transform.SetParent(transform);
            visualObj.transform.localPosition = Vector3.zero;

            fireVisual = visualObj.AddComponent<LineRenderer>();
            fireVisual.useWorldSpace = false;
            fireVisual.startWidth = 0.12f;
            fireVisual.endWidth = 0.12f;
            fireVisual.loop = true;
            fireVisual.material = new Material(Shader.Find("Sprites/Default"));
            fireVisual.startColor = new Color(1f, 0.4f, 0f, 0.6f);
            fireVisual.endColor = new Color(1f, 0.1f, 0f, 0.6f);

            int segments = 24;
            fireVisual.positionCount = segments;
            UpdateFireShape(1f);
        }

        private void UpdateFireShape(float scale)
        {
            if (fireVisual == null) return;
            int segments = fireVisual.positionCount;
            float baseRadius = 1.2f * scale;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                // Jagged flame-like shape
                float flicker = 1f + Mathf.Sin(angle * 5f + flickerTimer * 8f) * 0.2f;
                float r = baseRadius * flicker;
                fireVisual.SetPosition(i, new Vector3(
                    Mathf.Cos(angle) * r,
                    Mathf.Sin(angle) * r,
                    0f
                ));
            }
        }

        void Update()
        {
            if (healthHandler == null)
            {
                CleanupVisual();
                Destroy(this);
                return;
            }

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                CleanupVisual();
                Destroy(this);
                return;
            }

            flickerTimer += Time.deltaTime;
            UpdateFireShape(1f);

            // Fade out as burn expires
            if (fireVisual != null)
            {
                float alpha = Mathf.Clamp01(timer / duration) * 0.6f;
                fireVisual.startColor = new Color(1f, 0.4f, 0f, alpha);
                fireVisual.endColor = new Color(1f, 0.1f, 0f, alpha);
            }

            healthHandler.TakeDamage(Vector2.up * damagePerSecond * Time.deltaTime, transform.position);
        }

        private void CleanupVisual()
        {
            if (fireVisual != null)
            {
                Destroy(fireVisual.gameObject);
            }
        }

        void OnDestroy()
        {
            CleanupVisual();
        }
    }
}
