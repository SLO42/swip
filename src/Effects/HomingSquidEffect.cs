using UnityEngine;

namespace SWIP.Effects
{
    public class HomingSquidEffect : MonoBehaviour
    {
        public int squidCount = 2;
        public float squidDamage = 15f;
        public float squidSpeed = 8f;
        public float squidLifetime = 6f;
        public float inkRadius = 3f;
        public float inkDuration = 3f;
        public float inkSlowAmount = 0.5f;

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

            // Only spawn squids once per shot, not once per bullet
            int frame = Time.frameCount;
            if (frame == lastShotFrame) return;
            lastShotFrame = frame;

            for (int i = 0; i < squidCount; i++)
            {
                var squidObj = new GameObject("Squid");
                squidObj.transform.position = player.transform.position + new Vector3(
                    Random.Range(-0.8f, 0.8f),
                    Random.Range(0.5f, 1.5f),
                    0f
                );

                var rb = squidObj.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.mass = 0.08f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                var col = squidObj.AddComponent<CircleCollider2D>();
                col.radius = 0.15f;
                col.isTrigger = true;

                // Purple body
                var lr = squidObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.14f;
                lr.endWidth = 0.08f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(0f, -0.3f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(0.6f, 0.2f, 0.8f, 1f);
                lr.endColor = new Color(0.4f, 0.1f, 0.6f, 0.9f);
                lr.sortingOrder = 5;

                // Dark purple trail
                var trailObj = new GameObject("SquidTrail");
                trailObj.transform.SetParent(squidObj.transform);
                trailObj.transform.localPosition = Vector3.zero;
                var trail = trailObj.AddComponent<TrailRenderer>();
                trail.time = 0.4f;
                trail.startWidth = 0.1f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                trail.startColor = new Color(0.4f, 0.1f, 0.6f, 0.6f);
                trail.endColor = new Color(0.3f, 0.05f, 0.5f, 0f);
                trail.sortingOrder = 4;
                trail.minVertexDistance = 0.1f;

                var homing = squidObj.AddComponent<SquidHomingBehaviour>();
                homing.owner = player;
                homing.speed = squidSpeed;
                homing.damage = squidDamage;
                homing.lifetime = squidLifetime;
                homing.inkRadius = inkRadius;
                homing.inkDuration = inkDuration;
                homing.inkSlowAmount = inkSlowAmount;
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

    public class SquidHomingBehaviour : MonoBehaviour
    {
        public Player owner;
        public float speed = 8f;
        public float damage = 15f;
        public float lifetime = 6f;
        public float steerStrength = 15f;
        public float hitRadius = 1.5f;
        public float swimAmplitude = 1.5f;
        public float swimFrequency = 4f;
        public float inkRadius = 3f;
        public float inkDuration = 3f;
        public float inkSlowAmount = 0.5f;

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
                SpawnInkCloud();
                Destroy(gameObject);
                return;
            }

            Player target = FindClosestEnemy();
            if (target == null || rb == null) return;

            Vector2 toTarget = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

            // Sinusoidal oscillation perpendicular to velocity for swimming motion
            Vector2 perp = new Vector2(-toTarget.y, toTarget.x);
            float swim = Mathf.Sin(Time.time * swimFrequency) * swimAmplitude;
            Vector2 desired = toTarget + perp * swim * 0.25f;

            rb.velocity = Vector2.Lerp(rb.velocity.normalized, desired.normalized, Time.fixedDeltaTime * steerStrength) * speed;

            // Orient the visual along velocity
            if (lr != null && rb.velocity.sqrMagnitude > 0.1f)
            {
                Vector3 tail = ((Vector3)(-rb.velocity.normalized)) * 0.35f;
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
            // Ignore other squids
            if (other.GetComponent<SquidHomingBehaviour>() != null) return;

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

            SpawnInkCloud();
            Destroy(gameObject);
        }

        private void SpawnInkCloud()
        {
            var cloudObj = new GameObject("InkCloud");
            cloudObj.transform.position = transform.position;
            var zone = cloudObj.AddComponent<ZoneBehaviour>();
            zone.radius = inkRadius;
            zone.duration = inkDuration;
            zone.slowAmount = inkSlowAmount;
            zone.damagePerSecond = 0f;
            zone.outerColor = new Color(0.3f, 0.1f, 0.4f, 0.5f);
            zone.innerColor = new Color(0.2f, 0.05f, 0.3f, 0.35f);
            zone.owner = owner;
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
