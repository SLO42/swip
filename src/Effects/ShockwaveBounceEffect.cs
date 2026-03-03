using UnityEngine;

namespace SWIP.Effects
{
    public class ShockwaveBounceEffect : MonoBehaviour
    {
        public float pushRadius = 5f;
        public float pushForce = 1200f;

        private Vector3 lastPosition;
        private Vector3 lastDirection;
        private bool hasDirection;
        private float bounceCooldown;
        private ProjectileHit projectileHit;

        private void Start()
        {
            lastPosition = transform.position;
            projectileHit = GetComponent<ProjectileHit>();
        }

        private void FixedUpdate()
        {
            if (bounceCooldown > 0f) bounceCooldown -= Time.fixedDeltaTime;

            Vector3 currentPos = transform.position;
            Vector3 delta = currentPos - lastPosition;

            if (delta.sqrMagnitude > 0.01f)
            {
                Vector3 currentDir = delta.normalized;

                if (hasDirection && bounceCooldown <= 0f)
                {
                    float dot = Vector3.Dot(lastDirection, currentDir);
                    if (dot < 0.7f)
                    {
                        bounceCooldown = 0.15f;

                        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pushRadius);
                        foreach (var hit in hits)
                        {
                            Player player = hit.GetComponent<Player>();
                            if (player == null) continue;
                            if (player.data.dead) continue;
                            if (projectileHit != null && projectileHit.ownPlayer != null &&
                                player.teamID == projectileHit.ownPlayer.teamID) continue;

                            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                            if (playerRb != null)
                            {
                                Vector2 dir = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                                playerRb.AddForce(dir * pushForce);
                            }
                        }
                    }
                }

                lastDirection = currentDir;
                hasDirection = true;
            }

            lastPosition = currentPos;
        }
    }

    public class ShockwaveBounceSpawner : ProjectileEffectSpawner
    {
        public float pushRadius = 5f;
        public float pushForce = 1200f;

        protected override void ApplyToProjectile(GameObject projectile)
        {
            var effect = projectile.AddComponent<ShockwaveBounceEffect>();
            effect.pushRadius = pushRadius;
            effect.pushForce = pushForce;
        }
    }
}
