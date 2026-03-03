using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Lives on the player. Hooks gun to attach ScorchedSkyOnHit to every projectile.
    /// </summary>
    public class ScorchedSkySpawner : MonoBehaviour
    {
        public int projectileCount = 5;
        public float spreadWidth = 6f;
        public float fallSpeed = 40f;
        public float projectileDamage = 25f;
        public float explosionRange = 2f;
        public float explosionForce = 1500f;
        public int bounces = 0;

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
            if (!hooked) TryHookGun();
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
            var onHit = projectile.AddComponent<ScorchedSkyOnHit>();
            onHit.owner = player;
            onHit.projectileCount = projectileCount;
            onHit.spreadWidth = spreadWidth;
            onHit.fallSpeed = fallSpeed;
            onHit.projectileDamage = projectileDamage;
            onHit.explosionRange = explosionRange;
            onHit.explosionForce = explosionForce;
            onHit.bounces = bounces + (gun != null ? (int)gun.reflects : 0);
        }

        void OnDestroy()
        {
            if (gun != null)
            {
                gun.ShootPojectileAction -= OnShoot;
            }
        }
    }

    /// <summary>
    /// Lives on the projectile. Triggers sky rain on bullet hit.
    /// </summary>
    public class ScorchedSkyOnHit : MonoBehaviour
    {
        public Player owner;
        public int projectileCount = 5;
        public float spreadWidth = 6f;
        public float fallSpeed = 40f;
        public float projectileDamage = 25f;
        public float explosionRange = 2f;
        public float explosionForce = 1500f;
        public int bounces = 0;

        private bool triggered;

        void Start()
        {
            var projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                projectileHit.AddHitAction(OnHit);
            }
        }

        private void OnHit()
        {
            if (triggered) return;
            triggered = true;

            Vector3 hitPos = transform.position;

            // Find top of screen
            float screenTop = 30f;
            if (Camera.main != null)
            {
                screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y + 5f;
            }

            // Spawn projectiles in a horizontal line at the top
            float halfWidth = spreadWidth * 0.5f;
            for (int i = 0; i < projectileCount; i++)
            {
                float xOffset = projectileCount == 1 ? 0f :
                    Mathf.Lerp(-halfWidth, halfWidth, (float)i / (projectileCount - 1));

                Vector3 spawnPos = new Vector3(hitPos.x + xOffset, screenTop, 0f);

                var obj = new GameObject("SkyProjectile");
                obj.transform.position = spawnPos;

                var rb = obj.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.mass = 0.5f;
                rb.velocity = Vector2.down * fallSpeed;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                var col = obj.AddComponent<CircleCollider2D>();
                col.radius = 0.12f;
                col.isTrigger = true;

                // Visual: bright falling streak (larger for impact)
                var lr = obj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.18f;
                lr.endWidth = 0.05f;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(0f, 0.8f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(1f, 0.8f, 0.3f, 1f);
                lr.endColor = new Color(1f, 0.4f, 0f, 0.5f);
                lr.sortingOrder = 5;

                // Exhaust trail (longer, brighter)
                var trailObj = new GameObject("Trail");
                trailObj.transform.SetParent(obj.transform);
                trailObj.transform.localPosition = Vector3.zero;
                var trail = trailObj.AddComponent<TrailRenderer>();
                trail.time = 0.5f;
                trail.startWidth = 0.14f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                trail.startColor = new Color(1f, 0.6f, 0.1f, 0.8f);
                trail.endColor = new Color(1f, 0.2f, 0f, 0f);
                trail.sortingOrder = 4;
                trail.minVertexDistance = 0.1f;

                var proj = obj.AddComponent<SkyProjectileBehaviour>();
                proj.owner = owner;
                proj.damage = projectileDamage;
                proj.explosionRange = explosionRange;
                proj.explosionForce = explosionForce;
                proj.bouncesLeft = bounces;
                proj.fallSpeed = fallSpeed;
            }
        }
    }

    /// <summary>
    /// Falling projectile from Scorched Sky. Explodes on contact,
    /// spawns cloud effects from owner, and can bounce.
    /// </summary>
    public class SkyProjectileBehaviour : MonoBehaviour
    {
        public Player owner;
        public float damage = 20f;
        public float explosionRange = 1.5f;
        public float explosionForce = 800f;
        public int bouncesLeft = 0;
        public float fallSpeed = 25f;
        public float lifetime = 4f;

        private float timer;
        private bool exploded;
        private Rigidbody2D rb;

        void Start()
        {
            timer = lifetime;
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                Explode();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (exploded) return;

            // Ignore other sky projectiles and missiles
            if (other.GetComponent<SkyProjectileBehaviour>() != null) return;
            if (other.GetComponent<MissileHomingBehaviour>() != null) return;

            // Ignore owner
            var player = other.GetComponentInParent<Player>();
            if (player != null && owner != null && player.teamID == owner.teamID) return;

            // Damage destructables directly on hit
            var damagable = other.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.CallTakeDamage(Vector2.down * damage * 2f, transform.position);
            }

            // Bounce off non-player surfaces
            if (player == null && bouncesLeft > 0)
            {
                bouncesLeft--;
                if (rb != null)
                {
                    rb.velocity = new Vector2(
                        UnityEngine.Random.Range(-fallSpeed * 0.3f, fallSpeed * 0.3f),
                        fallSpeed * 0.6f
                    );
                }
                return;
            }

            Explode();
        }

        private void Explode()
        {
            if (exploded) return;
            exploded = true;

            Vector3 pos = transform.position;

            SWIPExplosion.Spawn(pos, damage, explosionRange, explosionForce, owner, SWIPExplosion.FireColors);

            // Inherit cloud/gas effects from owner
            if (owner != null)
            {
                foreach (var spawner in owner.GetComponentsInChildren<CloudEffectSpawner>())
                {
                    var cloudObj = new GameObject("Zone");
                    cloudObj.transform.position = pos;
                    var zone = cloudObj.AddComponent<ZoneBehaviour>();
                    zone.radius = spawner.cloudRadius * 0.6f;
                    zone.duration = spawner.cloudDuration * 0.5f;
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
    }
}
