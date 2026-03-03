using UnityEngine;

namespace SWIP.Effects
{
    public class MissileBarrageEffect : MonoBehaviour
    {
        public int missileCount = 3;
        public float missileDamage = 25f;
        public float missileSpeed = 15f;
        public float missileLifetime = 5f;
        public float explosionRange = 2.5f;
        public float explosionForce = 1500f;

        private Gun gun;
        private Player player;
        private bool hooked;

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

                // Visual: small line as missile body
                var lr = missileObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.15f;
                lr.endWidth = 0.05f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(0f, -0.4f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(1f, 0.6f, 0.1f, 1f);
                lr.endColor = new Color(1f, 0.2f, 0f, 0.6f);
                lr.sortingOrder = 5;

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
                Destroy(gameObject);
                return;
            }

            Player target = FindClosestEnemy();
            if (target == null || rb == null) return;

            Vector2 dir = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            rb.velocity = Vector2.Lerp(rb.velocity.normalized, dir, Time.fixedDeltaTime * steerStrength) * speed;

            // Orient the visual along velocity
            if (lr != null && rb.velocity.sqrMagnitude > 0.1f)
            {
                Vector3 tail = ((Vector3)(-rb.velocity.normalized)) * 0.4f;
                lr.SetPosition(1, tail);
            }

            // Proximity-based hit detection (more reliable than OnCollisionEnter2D)
            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist < hitRadius)
            {
                Explode();
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (graceTimer > 0f) return;

            Explode();
        }

        private void Explode()
        {
            if (exploded) return;
            exploded = true;

            // Kill physics immediately so nothing flings around
            var col = GetComponent<CircleCollider2D>();
            if (col != null) col.enabled = false;
            if (rb != null) { rb.velocity = Vector2.zero; rb.isKinematic = true; }
            if (lr != null) lr.enabled = false;

            // AoE damage + knockback to enemies in range
            Vector2 pos = transform.position;
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, explosionRange);
            foreach (var hit in hits)
            {
                var player = hit.GetComponentInParent<Player>();
                if (player == null || player.data.dead) continue;
                if (owner != null && player.teamID == owner.teamID) continue;

                Vector2 dir = ((Vector2)player.transform.position - pos).normalized;
                player.data.healthHandler.TakeDamage(dir * damage, pos);

                var playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.AddForce(dir * explosionForce);
                }
            }

            // Visual: expanding flash ring
            var flashObj = new GameObject("ExplosionFlash");
            flashObj.transform.position = transform.position;
            var flash = flashObj.AddComponent<LineRenderer>();
            flash.useWorldSpace = true;
            flash.loop = true;
            flash.widthMultiplier = 0.2f;
            flash.material = new Material(Shader.Find("Sprites/Default"));
            flash.startColor = new Color(1f, 0.6f, 0.1f, 1f);
            flash.endColor = new Color(1f, 0.3f, 0f, 0.8f);
            flash.sortingOrder = 6;
            int segments = 24;
            flash.positionCount = segments;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                float r = explosionRange * 0.3f;
                flash.SetPosition(i, (Vector3)pos + new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
            }
            var flashAnim = flashObj.AddComponent<ExplosionFlashAnim>();
            flashAnim.center = pos;
            flashAnim.maxRadius = explosionRange;

            // Spawn cloud/gas zones from the owner's cloud effect cards (stacks)
            if (owner != null)
            {
                foreach (var spawner in owner.GetComponentsInChildren<CloudEffectSpawner>())
                {
                    var cloudObj = new GameObject("Zone");
                    cloudObj.transform.position = transform.position;
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

    public class ExplosionFlashAnim : MonoBehaviour
    {
        public Vector2 center;
        public float maxRadius = 2.5f;
        public float duration = 0.25f;

        private float timer;
        private LineRenderer lr;

        void Start()
        {
            timer = 0f;
            lr = GetComponent<LineRenderer>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            if (t >= 1f)
            {
                Destroy(gameObject);
                return;
            }

            // Expand ring and fade out
            float r = Mathf.Lerp(maxRadius * 0.3f, maxRadius, t);
            float alpha = 1f - t;
            lr.startColor = new Color(1f, 0.6f, 0.1f, alpha);
            lr.endColor = new Color(1f, 0.3f, 0f, alpha * 0.8f);
            lr.widthMultiplier = 0.2f * (1f - t * 0.5f);

            int segments = lr.positionCount;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                lr.SetPosition(i, (Vector3)center + new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
            }
        }
    }
}
