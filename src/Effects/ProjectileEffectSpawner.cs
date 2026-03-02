using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Base class for player-attached components that hook gun.ShootPojectileAction
    /// to add effects to each fired projectile. Replaces the broken AddToProjectile pattern.
    /// </summary>
    public abstract class ProjectileEffectSpawner : MonoBehaviour
    {
        private Gun gun;
        private bool hooked;

        void Start()
        {
            TryHookGun();
        }

        void Update()
        {
            if (!hooked) TryHookGun();
        }

        private void TryHookGun()
        {
            var player = GetComponentInParent<Player>();
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
                gun.ShootPojectileAction += OnProjectileSpawned;
                hooked = true;
            }
        }

        void OnDestroy()
        {
            if (gun != null)
            {
                gun.ShootPojectileAction -= OnProjectileSpawned;
            }
        }

        private void OnProjectileSpawned(GameObject projectile)
        {
            ApplyToProjectile(projectile);
        }

        protected abstract void ApplyToProjectile(GameObject projectile);
    }
}
