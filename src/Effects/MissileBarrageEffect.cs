using UnityEngine;

namespace SWIP.Effects
{
    public class MissileBarrageEffect : MonoBehaviour
    {
        public int missileCount = 3;
        public float missileDamage = 25f;
        public float missileSpeed = 20f;
        public float missileLifetime = 5f;
        public float explosionRange = 2.5f;
        public float explosionForce = 1500f;

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

            // Only spawn missiles once per shot, not once per bullet
            int frame = Time.frameCount;
            if (frame == lastShotFrame) return;
            lastShotFrame = frame;

            for (int i = 0; i < missileCount; i++)
            {
                var missileObj = new GameObject("Missile");
                missileObj.transform.position = player.transform.position + new Vector3(
                    UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(0.5f, 2f),
                    0f
                );

                var rb = missileObj.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.mass = 0.1f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                var col = missileObj.AddComponent<CircleCollider2D>();
                col.radius = 0.15f;
                col.isTrigger = true;

                // Visual: missile body (bright tip)
                var lr = missileObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.12f;
                lr.endWidth = 0.04f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(0f, -0.3f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(1f, 0.9f, 0.7f, 1f);
                lr.endColor = new Color(1f, 0.5f, 0.1f, 0.9f);
                lr.sortingOrder = 5;

                // Exhaust trail (child GO with its own LineRenderer tracking positions)
                var trailObj = new GameObject("Exhaust");
                trailObj.transform.SetParent(missileObj.transform);
                trailObj.transform.localPosition = Vector3.zero;
                var trail = trailObj.AddComponent<TrailRenderer>();
                trail.time = 0.4f;
                trail.startWidth = 0.12f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                trail.startColor = new Color(1f, 0.4f, 0f, 0.7f);
                trail.endColor = new Color(1f, 0.2f, 0f, 0f);
                trail.sortingOrder = 4;
                trail.minVertexDistance = 0.1f;

                var homing = missileObj.AddComponent<MissileHomingBehaviour>();
                homing.owner = player;
                homing.speed = missileSpeed;
                homing.damage = missileDamage;
                homing.lifetime = missileLifetime;
                homing.explosionRange = explosionRange;
                homing.explosionForce = explosionForce;
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

    public class MissileHomingBehaviour : MonoBehaviour
    {
        public Player owner;
        public float speed = 15f;
        public float damage = 25f;
        public float lifetime = 5f;
        public float explosionRange = 2.5f;
        public float explosionForce = 1500f;
        public float steerStrength = 10f;
        public float hitRadius = 1.5f;
        public float graceTime = 0.3f;

        private float timer;
        private float graceTimer;
        private bool exploded;
        private Rigidbody2D rb;
        private LineRenderer lr;

        void Start()
        {
            timer = lifetime;
            graceTimer = graceTime;
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
            if (graceTimer > 0f) graceTimer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                Explode();
                return;
            }

            Player target = FindClosestEnemy();
            if (target == null || rb == null) return;

            // Steer toward target center
            Vector2 dir = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            rb.velocity = Vector2.Lerp(rb.velocity.normalized, dir, Time.fixedDeltaTime * steerStrength) * speed;

            // Orient the visual along velocity
            if (lr != null && rb.velocity.sqrMagnitude > 0.1f)
            {
                Vector3 tail = ((Vector3)(-rb.velocity.normalized)) * 0.4f;
                lr.SetPosition(1, tail);
            }

            // Proximity hit — check distance to target center directly
            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist < hitRadius)
            {
                Explode();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Always ignore other missiles
            if (other.GetComponent<MissileHomingBehaviour>() != null) return;

            // Always ignore owner's team
            var player = other.GetComponentInParent<Player>();
            if (player != null && owner != null && player.teamID == owner.teamID) return;

            // During grace period, only skip owner's bullets — still hit everything else
            if (graceTimer > 0f)
            {
                var projHit = other.GetComponent<ProjectileHit>();
                if (projHit == null) projHit = other.GetComponentInParent<ProjectileHit>();
                if (projHit != null) return;
            }

            Explode();
        }

        private void Explode()
        {
            if (exploded) return;
            exploded = true;

            Vector3 pos = transform.position;

            SWIPExplosion.Spawn(pos, damage, explosionRange, explosionForce, owner, SWIPExplosion.MissileColors);

            // Spawn cloud/gas zones from the owner's cloud effect cards (stacks)
            if (owner != null)
            {
                foreach (var spawner in owner.GetComponentsInChildren<CloudEffectSpawner>())
                {
                    var cloudObj = new GameObject("Zone");
                    cloudObj.transform.position = pos;
                    var zone = cloudObj.AddComponent<ZoneBehaviour>();
                    zone.radius = spawner.cloudRadius;
                    zone.duration = spawner.cloudDuration;
                    zone.damagePerSecond = spawner.damagePerSecond;
                    zone.healPerSecond = spawner.healPerSecond;
                    zone.slowAmount = spawner.slowAmount;
                    zone.outerColor = spawner.outerColor;
                    zone.innerColor = spawner.innerColor;
                    zone.owner = owner;
                    zone.affectsOwner = spawner.affectsOwner;
                }
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
