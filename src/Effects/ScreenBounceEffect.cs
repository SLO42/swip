using UnityEngine;

namespace SWIP.Effects
{
    public class ScreenBounceEffect : MonoBehaviour
    {
        private void FixedUpdate()
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            Vector3 worldPos = transform.position;
            Vector3 vp = cam.WorldToViewportPoint(worldPos);
            bool reflected = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            MoveTransform mt = GetComponent<MoveTransform>();

            if (vp.x < 0f)
            {
                vp.x = 0f;
                if (rb != null) rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);
                if (mt != null) mt.velocity = new Vector3(Mathf.Abs(mt.velocity.x), mt.velocity.y, mt.velocity.z);
                reflected = true;
            }
            else if (vp.x > 1f)
            {
                vp.x = 1f;
                if (rb != null) rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y);
                if (mt != null) mt.velocity = new Vector3(-Mathf.Abs(mt.velocity.x), mt.velocity.y, mt.velocity.z);
                reflected = true;
            }

            if (vp.y < 0f)
            {
                vp.y = 0f;
                if (rb != null) rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));
                if (mt != null) mt.velocity = new Vector3(mt.velocity.x, Mathf.Abs(mt.velocity.y), mt.velocity.z);
                reflected = true;
            }
            else if (vp.y > 1f)
            {
                vp.y = 1f;
                if (rb != null) rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y));
                if (mt != null) mt.velocity = new Vector3(mt.velocity.x, -Mathf.Abs(mt.velocity.y), mt.velocity.z);
                reflected = true;
            }

            if (reflected)
            {
                Vector3 clampedWorld = cam.ViewportToWorldPoint(vp);
                clampedWorld.z = worldPos.z;
                transform.position = clampedWorld;
            }
        }
    }

    public class ScreenBounceSpawner : ProjectileEffectSpawner
    {
        protected override void ApplyToProjectile(GameObject projectile)
        {
            projectile.AddComponent<ScreenBounceEffect>();
        }
    }
}
