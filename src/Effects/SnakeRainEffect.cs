using UnityEngine;

namespace SWIP.Effects
{
    public class SnakeRainEffect : MonoBehaviour
    {
        public int snakeCount = 3;
        public float snakeDamage = 15f;
        public float snakeSpeed = 8f;
        public float snakeLifetime = 4f;

        private Player player;
        private float lastHealth;

        void Start()
        {
            player = GetComponentInParent<Player>();
            if (player != null)
            {
                lastHealth = player.data.health;
            }
        }

        void LateUpdate()
        {
            if (player == null || player.data.dead) return;

            float currentHealth = player.data.health;
            if (currentHealth < lastHealth - 0.1f)
            {
                SpawnSnakes();
            }
            lastHealth = currentHealth;
        }

        private void SpawnSnakes()
        {
            for (int i = 0; i < snakeCount; i++)
            {
                var snakeObj = new GameObject("Snake");
                snakeObj.transform.position = player.transform.position + new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1.5f),
                    0f
                );

                var rb = snakeObj.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.mass = 0.05f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                var col = snakeObj.AddComponent<CircleCollider2D>();
                col.radius = 0.1f;
                col.isTrigger = true;

                // Green trail
                var trail = snakeObj.AddComponent<TrailRenderer>();
                trail.time = 0.5f;
                trail.startWidth = 0.08f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                trail.startColor = new Color(0.2f, 0.9f, 0.2f, 0.8f);
                trail.endColor = new Color(0.1f, 0.7f, 0.1f, 0f);
                trail.sortingOrder = 4;
                trail.minVertexDistance = 0.05f;

                // Small head visual
                var lr = snakeObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.1f;
                lr.endWidth = 0.06f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(0f, -0.15f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(0.1f, 0.8f, 0.1f, 1f);
                lr.endColor = new Color(0.2f, 0.6f, 0.1f, 0.8f);
                lr.sortingOrder = 5;

                var homing = snakeObj.AddComponent<SnakeHomingBehaviour>();
                homing.owner = player;
                homing.speed = snakeSpeed;
                homing.damage = snakeDamage;
                homing.lifetime = snakeLifetime;
            }
        }
    }

    public class SnakeHomingBehaviour : MonoBehaviour
    {
        public Player owner;
        public float speed = 8f;
        public float damage = 15f;
        public float lifetime = 4f;
        public float steerStrength = 8f;
        public float hitRadius = 1.2f;
        public float waveAmplitude = 2f;
        public float waveFrequency = 6f;

        private float timer;
        private bool hit;
        private Rigidbody2D rb;
        private LineRenderer lr;

        void Start()
        {
            timer = lifetime;
            rb = GetComponent<Rigidbody2D>();
            lr = GetComponent<LineRenderer>();

            if (rb != null)
            {
                rb.velocity = Random.insideUnitCircle.normalized * speed;
            }
        }

        void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            Player target = FindClosestEnemy();
            if (target == null || rb == null) return;

            Vector2 toTarget = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

            // Sinusoidal perpendicular oscillation for wavy snake movement
            Vector2 perp = new Vector2(-toTarget.y, toTarget.x);
            float wave = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            Vector2 desired = toTarget + perp * wave * 0.3f;

            rb.velocity = Vector2.Lerp(rb.velocity.normalized, desired.normalized, Time.fixedDeltaTime * steerStrength) * speed;

            // Orient the visual along velocity
            if (lr != null && rb.velocity.sqrMagnitude > 0.1f)
            {
                Vector3 tail = ((Vector3)(-rb.velocity.normalized)) * 0.2f;
                lr.SetPosition(1, tail);
            }

            // Proximity hit
            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist < hitRadius)
            {
                HitTarget(target);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Ignore other snakes
            if (other.GetComponent<SnakeHomingBehaviour>() != null) return;

            // Ignore owner's team
            var player = other.GetComponentInParent<Player>();
            if (player != null && owner != null && player.teamID == owner.teamID) return;

            if (player != null) HitTarget(player);
        }

        private void HitTarget(Player target)
        {
            if (hit) return;
            hit = true;

            if (target != null && !target.data.dead)
            {
                target.data.healthHandler.TakeDamage(Vector2.up * damage, transform.position);
            }

            Destroy(gameObject);
        }

        private Player FindClosestEnemy()
        {
            Player closest = null;
            float closestDist = float.MaxValue;

            foreach (var p in PlayerManager.instance.players)
            {
                if (p == owner) continue;
                if (p.data.dead) continue;
                if (owner != null && p.teamID == owner.teamID) continue;

                float dist = Vector2.Distance(transform.position, p.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = p;
                }
            }
            return closest;
        }
    }
}
