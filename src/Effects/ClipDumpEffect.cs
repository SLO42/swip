using UnityEngine;

namespace SWIP.Effects
{
    public class ClipDumpEffect : MonoBehaviour
    {
        public float speedMultiplier = 0.05f;

        private Gun gun;
        private Player player;
        private bool hooked;
        private float originalAttackSpeed;

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
                originalAttackSpeed = gun.attackSpeed;
                gun.attackSpeed *= speedMultiplier;
                hooked = true;
            }
        }

        void OnDestroy()
        {
            if (gun != null)
            {
                gun.attackSpeed = originalAttackSpeed;
            }
        }
    }
}
