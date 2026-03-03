using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Hooks gun.ShootPojectileAction to attach WaveMotionEffect to every projectile.
    /// </summary>
    public class WaveMotionSpawner : MonoBehaviour
    {
        public float waveAmplitude = 5f;
        public float waveFrequency = 4f;

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
            var wave = projectile.AddComponent<WaveMotionEffect>();
            wave.amplitude = waveAmplitude;
            wave.frequency = waveFrequency;
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
    /// Applies sine-wave lateral oscillation to a projectile in flight.
    /// Uses direct position offset so the forward velocity is never altered —
    /// the bullet travels its original path while weaving side-to-side.
    /// </summary>
    public class WaveMotionEffect : MonoBehaviour
    {
        public float amplitude = 5f;
        public float frequency = 4f;

        private Vector3 lateralDir;
        private float timer;
        private float currentOffset;
        private bool initialized;

        void FixedUpdate()
        {
            if (!initialized)
            {
                var mt = GetComponent<MoveTransform>();
                if (mt == null || mt.velocity.sqrMagnitude < 0.1f) return;

                Vector3 fwd = mt.velocity.normalized;
                // Perpendicular in 2D: rotate 90 degrees
                lateralDir = new Vector3(-fwd.y, fwd.x, 0f);
                initialized = true;
            }

            timer += Time.fixedDeltaTime;

            // Calculate new sine offset and apply the delta as position change
            float newOffset = amplitude * Mathf.Sin(timer * frequency * Mathf.PI * 2f);
            float delta = newOffset - currentOffset;
            currentOffset = newOffset;

            transform.position += lateralDir * delta;
        }
    }
}
