using UnityEngine;

namespace SWIP.Effects
{
    public class HomingBirdEffect : MonoBehaviour
    {
        public int birdCount = 2;
        public float birdDamage = 20f;
        public float birdSpeed = 12f;
        public float birdLifetime = 5f;

        private Gun gun;
        private Player player;
        private bool hooked;
        private int lastShotFrame = -1;

        void Start()
        {
            player = GetComponentInParent<Player>();
            TryHookGun();
        }

        void Update()
        {
            if (!hooked)
            {
                TryHookGun();
            }
        }

        private void TryHookGun()
        {
            if (player == null) return;

            Gun foundGun = null;
            var holding = player.GetComponent<Holding>();
            if (holding != null && holding.holdable != null)
            {
                foundGun = holding.holdable.GetComponent<Gun>();
            }
            if (foundGun == null)
            {
                foundGun = player.GetComponentInChildren<Gun>();
            }

            if (foundGun != null)
            {
                gun = foundGun;
                gun.ShootPojectileAction += OnShoot;
                hooked = true;
            }
        }

        private void OnShoot(GameObject projectile)
        {
            if (player == null) return;

            // Only spawn birds once per shot, not once per bullet
            int frame = Time.frameCount;
            if (frame == lastShotFrame) return;
            lastShotFrame = frame;

            for (int i = 0; i < birdCount; i++)
            {
                var birdObj = new GameObject("Bird");
                birdObj.transform.position = player.transform.position + new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(1f, 2f),
                    0f
                );

                var rb = birdObj.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.mass = 0.05f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                var col = birdObj.AddComponent<CircleCollider2D>();
                col.radius = 0.15f;
                col.isTrigger = true;

                // Bright red body
                var lr = birdObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.14f;
                lr.endWidth = 0.06f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(0f, -0.25f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(1f, 0.2f, 0.1f, 1f);
                lr.endColor = new Color(0.9f, 0.4f, 0.2f, 0.9f);
                lr.sortingOrder = 5;

                // Small trail
                var trailObj = new GameObject("BirdTrail");
                trailObj.transform.SetParent(birdObj.transform);
                trailObj.transform.localPosition = Vector3.zero;
                var trail = trailObj.AddComponent<TrailRenderer>();
                trail.time = 0.3f;
                trail.startWidth = 0.1f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                trail.startColor = new Color(1f, 0.3f, 0.1f, 0.6f);
                trail.endColor = new Color(1f, 0.5f, 0.2f, 0f);
                trail.sortingOrder = 4;
                trail.minVertexDistance = 0.1f;

                var homing = birdObj.AddComponent<BirdHomingBehaviour>();
                homing.owner = player;
                homing.speed = birdSpeed;
                homing.damage = birdDamage;
                homing.lifetime = birdLifetime;
            }
        }

        void OnDestroy()
        {
            if (gun != null)
            {
                gun.ShootPojectileAction -= OnShoot;
            }
        }
    }

    public class BirdHomingBehaviour : MonoBehaviour
    {
        public Player owner;
        public float speed = 12f;
        public float damage = 20f;
        public float lifetime = 5f;
        public float steerStrength = 10f;
        public float hitRadius = 1.5f;
        public float swoopAmplitude = 1.5f;
        public float swoopFrequency = 3f;

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
                rb.velocity = Vector2.up * speed * 0.5f;
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

            // Swooping arc: sine wave vertical oscillation
            float swoop = Mathf.Sin(Time.time * swoopFrequency) * swoopAmplitude;
            Vector2 perpendicular = new Vector2(-toTarget.y, toTarget.x);
            Vector2 desired = toTarget + perpendicular * swoop * 0.2f;

            rb.velocity = Vector2.Lerp(rb.velocity.normalized, desired.normalized, Time.fixedDeltaTime * steerStrength) * speed;

            // Orient the visual along velocity
            if (lr != null && rb.velocity.sqrMagnitude > 0.1f)
            {
                Vector3 tail = ((Vector3)(-rb.velocity.normalized)) * 0.3f;
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
            // Ignore other birds
            if (other.GetComponent<BirdHomingBehaviour>() != null) return;

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
