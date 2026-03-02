using System;
using UnityEngine;

namespace SWIP.Effects
{
    public class SideshootEffect : MonoBehaviour
    {
        public float angleRange = 60f;

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
            if (!hooked)
            {
                TryHookGun();
            }
        }

        private void TryHookGun()
        {
            if (player == null) return;

            // Try multiple paths to find the gun
            Gun foundGun = null;

            // Path 1: Through Holding component
            var holding = player.GetComponent<Holding>();
            if (holding != null && holding.holdable != null)
            {
                foundGun = holding.holdable.GetComponent<Gun>();
            }

            // Path 2: Through player's children
            if (foundGun == null)
            {
                foundGun = player.GetComponentInChildren<Gun>();
            }

            if (foundGun != null)
            {
                gun = foundGun;
                gun.ShootPojectileAction += OnShootProjectile;
                hooked = true;
            }
        }

        private void OnShootProjectile(GameObject obj)
        {
            var moveTransform = obj.GetComponent<MoveTransform>();
            if (moveTransform != null)
            {
                float randomAngle = UnityEngine.Random.Range(-angleRange, angleRange);
                float rad = randomAngle * Mathf.Deg2Rad;
                Vector3 vel = moveTransform.velocity;
                float cos = Mathf.Cos(rad);
                float sin = Mathf.Sin(rad);
                moveTransform.velocity = new Vector3(
                    vel.x * cos - vel.y * sin,
                    vel.x * sin + vel.y * cos,
                    vel.z
                );
            }
        }

        void OnDestroy()
        {
            if (gun != null)
            {
                gun.ShootPojectileAction -= OnShootProjectile;
            }
        }
    }
}
