using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Lives on the player. Hooks gun.ShootPojectileAction to attach
    /// TerrainBreakerEffect to every fired projectile.
    /// </summary>
    public class TerrainBreakerSpawner : MonoBehaviour
    {
        public float breakRadius = 3f;
        public float breakDamage = 1000f;

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
            var breaker = projectile.AddComponent<TerrainBreakerEffect>();
            breaker.breakRadius = breakRadius;
            breaker.breakDamage = breakDamage;
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
    /// Lives on the projectile. On hit: destroys only the specific object hit
    /// (not everything in a radius) and spawns an explosion for visual + knockback.
    /// </summary>
    public class TerrainBreakerEffect : MonoBehaviour
    {
        public float breakRadius = 3f;
        public float breakDamage = 1000f;

        void Start()
        {
            var projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                projectileHit.AddHitActionWithData(OnHitWithData);
            }
        }

        private void OnHitWithData(HitInfo hitInfo)
        {
            Vector3 pos = transform.position;

            // Visual explosion + knockback
            SWIPExplosion.Spawn(pos, breakDamage, breakRadius, 2000f);

            // Destroy only the specific object that was hit (terrain/props only)
            if (hitInfo.collider != null)
            {
                var hitObj = hitInfo.collider.gameObject;
                var root = hitObj.transform.root.gameObject;
                if (root.GetComponentInChildren<Player>() == null &&
                    hitObj.GetComponentInParent<Player>() == null &&
                    hitObj.GetComponentInParent<ProjectileHit>() == null)
                {
                    var damagable = hitObj.GetComponent<Damagable>();
                    if (damagable != null)
                    {
                        damagable.CallTakeDamage(Vector2.up * breakDamage, pos);
                    }

                    Object.Destroy(hitObj);
                }
            }
        }
    }
}
