using UnityEngine;

namespace SWIP.Effects
{
    public class RadialShotEffect : MonoBehaviour
    {
        private Gun gun;
        private Player player;
        private bool hooked;
        private int lastShotFrame = -1;
        private int projectileIndex = 0;

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
            if (player == null || gun == null) return;

            int frame = Time.frameCount;
            if (frame != lastShotFrame)
            {
                lastShotFrame = frame;
                projectileIndex = 0;
            }

            int totalBullets = Mathf.Max(1, gun.numberOfProjectiles);
            float angle = (360f / totalBullets) * projectileIndex;
            projectileIndex++;

            // Rotate the projectile's velocity
            var mt = projectile.GetComponent<MoveTransform>();
            if (mt != null)
            {
                mt.velocity = Quaternion.Euler(0, 0, angle) * mt.velocity;
            }
            var rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Quaternion.Euler(0, 0, angle) * rb.velocity;
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
}
