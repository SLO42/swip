using UnityEngine;

namespace SWIP.Effects
{
    public class GravityAuraEffect : MonoBehaviour
    {
        public float radius = 15f;
        public float strength = 30f;

        private Player owner;
        private LineRenderer auraVisual;
        private float pulseTimer;

        void Start()
        {
            owner = GetComponentInParent<Player>();
            CreateVisual();
        }

        private void CreateVisual()
        {
            var visualObj = new GameObject("GravityAuraVisual");
            visualObj.transform.SetParent(transform);
            visualObj.transform.localPosition = Vector3.zero;

            auraVisual = visualObj.AddComponent<LineRenderer>();
            auraVisual.useWorldSpace = false;
            auraVisual.startWidth = 0.08f;
            auraVisual.endWidth = 0.08f;
            auraVisual.loop = true;
            auraVisual.material = new Material(Shader.Find("Sprites/Default"));
            auraVisual.startColor = new Color(0.6f, 0.2f, 0.8f, 0.3f);
            auraVisual.endColor = new Color(0.6f, 0.2f, 0.8f, 0.3f);

            int segments = 40;
            auraVisual.positionCount = segments;
            UpdateCircle(radius);
        }

        private void UpdateCircle(float r)
        {
            if (auraVisual == null) return;
            int segments = auraVisual.positionCount;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                auraVisual.SetPosition(i, new Vector3(
                    Mathf.Cos(angle) * r,
                    Mathf.Sin(angle) * r,
                    0f
                ));
            }
        }

        void FixedUpdate()
        {
            if (owner == null) return;

            // Pulse the visual
            pulseTimer += Time.fixedDeltaTime;
            float pulse = 1f + Mathf.Sin(pulseTimer * 2f) * 0.05f;
            UpdateCircle(radius * pulse);

            float alpha = 0.2f + Mathf.Sin(pulseTimer * 3f) * 0.1f;
            if (auraVisual != null)
            {
                Color c = new Color(0.6f, 0.2f, 0.8f, alpha);
                auraVisual.startColor = c;
                auraVisual.endColor = c;
            }

            var players = PlayerManager.instance.players;
            for (int i = 0; i < players.Count; i++)
            {
                var target = players[i];
                if (target == owner) continue;
                if (target.data.dead) continue;

                Vector2 diff = (Vector2)owner.transform.position - (Vector2)target.transform.position;
                float dist = diff.magnitude;
                if (dist > radius || dist < 0.5f) continue;

                // Linear falloff: strongest at close range, zero at edge of radius
                float pullAccel = strength * (1f - dist / radius);
                Vector2 pullDir = diff.normalized;

                // Directly modify velocity — bypasses mass and movement controller resistance
                var rb = target.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity += pullDir * pullAccel * Time.fixedDeltaTime;
                }
            }
        }

        void OnDestroy()
        {
            if (auraVisual != null)
            {
                Destroy(auraVisual.gameObject);
            }
        }
    }
}
