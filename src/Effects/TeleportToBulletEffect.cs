using UnityEngine;

namespace SWIP.Effects
{
    public class TeleportToBulletEffect : MonoBehaviour
    {
        private ProjectileHit projectileHit;
        private Player owner;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                owner = projectileHit.ownPlayer;
                projectileHit.AddHitAction(TeleportOwner);
            }
        }

        private void TeleportOwner()
        {
            if (owner != null && !owner.data.dead)
            {
                owner.transform.position = transform.position;
            }
        }
    }

    public class TeleportToBulletSpawner : ProjectileEffectSpawner
    {
        protected override void ApplyToProjectile(GameObject projectile)
        {
            projectile.AddComponent<TeleportToBulletEffect>();
        }
    }
}
