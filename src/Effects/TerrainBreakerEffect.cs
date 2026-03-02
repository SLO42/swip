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
    /// Lives on the projectile. Destroys terrain on bullet death.
    /// </summary>
    public class TerrainBreakerEffect : MonoBehaviour
    {
        public float breakRadius = 3f;
        public float breakDamage = 1000f;

        private bool triggered;

        void OnDestroy()
        {
            if (!triggered)
            {
                BreakTerrain();
            }
        }

        private void BreakTerrain()
        {
            triggered = true;
            Vector3 pos = transform.position;

            var expObj = new GameObject("TerrainExplosion");
            expObj.transform.position = pos;
            var exp = expObj.AddComponent<Explosion>();
            exp.auto = true;
            exp.damage = breakDamage;
            exp.range = breakRadius;
            exp.force = 2000f;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, breakRadius);
            foreach (var col in colliders)
            {
                if (col == null) continue;
                if (col.GetComponentInParent<Player>() != null) continue;
                if (col.GetComponentInParent<ProjectileHit>() != null) continue;

                var damagable = col.GetComponent<Damagable>();
                if (damagable != null)
                {
                    damagable.CallTakeDamage(Vector2.up * breakDamage, pos);
                }

                Object.Destroy(col.gameObject);
            }
        }
    }
}
