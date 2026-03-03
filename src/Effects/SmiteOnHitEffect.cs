using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Lives on the player. Hooks gun to attach SmiteBeamOnHit to every projectile.
    /// </summary>
    public class SmiteBeamSpawner : MonoBehaviour
    {
        public float beamDamage = 55f;
        public float beamWidth = 1.5f;

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
            var onHit = projectile.AddComponent<SmiteBeamOnHit>();
            onHit.owner = player;
            onHit.beamDamage = beamDamage;
            onHit.beamWidth = beamWidth;
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
    /// Lives on the projectile. On hit: spawns a holy beam from the top of the
    /// screen that penetrates through EVERYTHING, dealing damage along the path.
    /// </summary>
    public class SmiteBeamOnHit : MonoBehaviour
    {
        public Player owner;
        public float beamDamage = 55f;
        public float beamWidth = 1.5f;

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

            float screenTop = 30f;
            float screenBottom = -30f;
            if (Camera.main != null)
            {
                screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y + 5f;
                screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).y - 5f;
            }

            // Damage everything in the beam column using OverlapArea
            Vector2 pointA = new Vector2(hitPos.x - beamWidth * 0.5f, screenTop);
            Vector2 pointB = new Vector2(hitPos.x + beamWidth * 0.5f, screenBottom);

            Collider2D[] hits = Physics2D.OverlapAreaAll(pointA, pointB);
            foreach (var hit in hits)
            {
                // Skip other projectiles and beams
                if (hit.GetComponent<ProjectileHit>() != null) continue;
                if (hit.GetComponentInParent<ProjectileHit>() != null) continue;

                // Damage players (skip owner's team)
                var player = hit.GetComponentInParent<Player>();
                if (player != null)
                {
                    if (!player.data.dead && (owner == null || player.teamID != owner.teamID))
                    {
                        player.data.healthHandler.TakeDamage(Vector2.down * beamDamage, hit.transform.position);
                    }
                    continue;
                }

                // Damage destructables
                var damagable = hit.GetComponent<Damagable>();
                if (damagable != null)
                {
                    damagable.CallTakeDamage(Vector2.down * beamDamage * 2f, hit.transform.position);
                }
            }

            // Spawn visual
            var beamObj = new GameObject("SmiteBeam");
            beamObj.transform.position = new Vector3(hitPos.x, 0f, 0f);
            var visual = beamObj.AddComponent<SmiteBeamVisual>();
            visual.topY = screenTop;
            visual.bottomY = screenBottom;
            visual.beamWidth = beamWidth;
            visual.targetX = hitPos.x;
        }
    }

    /// <summary>
    /// Visual for the holy smite beam — golden light column that fades out.
    /// </summary>
    public class SmiteBeamVisual : MonoBehaviour
    {
        public float topY;
        public float bottomY;
        public float beamWidth = 1.5f;
        public float targetX;
        public float duration = 0.3f;

        private float timer;
        private LineRenderer beam;
        private LineRenderer glow;

        void Start()
        {
            // Main beam — bright gold/white core
            beam = gameObject.AddComponent<LineRenderer>();
            beam.useWorldSpace = true;
            beam.startWidth = beamWidth;
            beam.endWidth = beamWidth * 0.8f;
            beam.positionCount = 2;
            beam.SetPosition(0, new Vector3(targetX, topY, 0f));
            beam.SetPosition(1, new Vector3(targetX, bottomY, 0f));
            beam.material = new Material(Shader.Find("Sprites/Default"));
            beam.startColor = new Color(1f, 0.95f, 0.6f, 0.9f);
            beam.endColor = new Color(1f, 0.85f, 0.4f, 0.7f);
            beam.sortingOrder = 12;

            // Outer glow — soft golden haze
            var glowObj = new GameObject("Glow");
            glowObj.transform.SetParent(transform);
            glow = glowObj.AddComponent<LineRenderer>();
            glow.useWorldSpace = true;
            glow.startWidth = beamWidth * 2.5f;
            glow.endWidth = beamWidth * 2f;
            glow.positionCount = 2;
            glow.SetPosition(0, new Vector3(targetX, topY, 0f));
            glow.SetPosition(1, new Vector3(targetX, bottomY, 0f));
            glow.material = new Material(Shader.Find("Sprites/Default"));
            glow.startColor = new Color(1f, 0.9f, 0.3f, 0.3f);
            glow.endColor = new Color(1f, 0.8f, 0.2f, 0.15f);
            glow.sortingOrder = 11;
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

            float alpha = 1f - t;

            beam.startColor = new Color(1f, 0.95f, 0.6f, 0.9f * alpha);
            beam.endColor = new Color(1f, 0.85f, 0.4f, 0.7f * alpha);
            beam.startWidth = beamWidth * (1f + t * 0.3f);
            beam.endWidth = beamWidth * 0.8f * (1f + t * 0.3f);

            glow.startColor = new Color(1f, 0.9f, 0.3f, 0.3f * alpha);
            glow.endColor = new Color(1f, 0.8f, 0.2f, 0.15f * alpha);
            glow.startWidth = beamWidth * 2.5f * (1f + t * 0.5f);
            glow.endWidth = beamWidth * 2f * (1f + t * 0.5f);
        }
    }
}
