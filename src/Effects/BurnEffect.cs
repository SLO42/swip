using UnityEngine;

namespace SWIP.Effects
{
    public class BurnEffect : MonoBehaviour
    {
        public float burnDPS = 5f;
        public float burnDuration = 3f;

        private ProjectileHit projectileHit;
        private Player owner;

        void Start()
        {
            projectileHit = GetComponentInParent<ProjectileHit>();
            if (projectileHit == null) projectileHit = GetComponent<ProjectileHit>();
            if (projectileHit != null)
            {
                owner = projectileHit.ownPlayer;
                projectileHit.AddHitAction(ApplyBurn);
            }
        }

        private void ApplyBurn()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (var hit in hits)
            {
                var player = hit.GetComponentInParent<Player>();
                if (player != null && !player.data.dead && player != owner)
                {
                    var burn = player.gameObject.AddComponent<BurnDamageEffect>();
                    burn.damagePerSecond = burnDPS;
                    burn.duration = burnDuration;
                }
            }
        }
    }

    public class BurnEffectSpawner : ProjectileEffectSpawner
    {
        public float burnDPS = 5f;
        public float burnDuration = 3f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<BurnEffect>();
            effect.burnDPS = burnDPS;
            effect.burnDuration = burnDuration;
        }
    }
}
