using UnityEngine;
using System.Collections;

namespace SWIP.Effects
{
    public class LaserBeamEffect : MonoBehaviour
    {
        public float laserDamage = 55f;
        public float laserRange = 100f;
        public float beamDuration = 0.3f;
        public float beamWidth = 0.5f;
        public Color beamColor = new Color(1f, 0.2f, 0.2f, 0.9f);
        public Color beamCoreColor = new Color(1f, 1f, 1f, 1f);

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
            if (player == null) return;

            Vector3 gunPos = gun.transform.position;
            Vector3 aimDir = gun.transform.forward;

            var moveTransform = projectile.GetComponent<MoveTransform>();
            if (moveTransform != null)
            {
                aimDir = moveTransform.velocity.normalized;
            }

            Vector2 origin = gunPos;
            Vector2 direction = aimDir;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, laserRange);

            Vector3 endPoint = origin + direction * laserRange;
            if (hit.collider != null)
            {
                endPoint = hit.point;
                var hitPlayer = hit.collider.GetComponentInParent<Player>();
                if (hitPlayer != null && hitPlayer != player && !hitPlayer.data.dead)
                {
                    hitPlayer.data.healthHandler.TakeDamage(Vector2.up * laserDamage, hit.point);
                }
            }

            // Wide outer glow
            var beamObj = new GameObject("LaserBeam");
            var glowLr = beamObj.AddComponent<LineRenderer>();
            glowLr.startWidth = beamWidth * 2.5f;
            glowLr.endWidth = beamWidth * 2.5f;
            glowLr.positionCount = 2;
            glowLr.SetPosition(0, gunPos);
            glowLr.SetPosition(1, endPoint);
            glowLr.material = new Material(Shader.Find("Sprites/Default"));
            glowLr.startColor = new Color(beamColor.r, beamColor.g, beamColor.b, 0.15f);
            glowLr.endColor = new Color(beamColor.r, beamColor.g, beamColor.b, 0.1f);
            glowLr.sortingOrder = 9;

            // Main beam
            var mainObj = new GameObject("LaserMain");
            mainObj.transform.SetParent(beamObj.transform);
            var mainLr = mainObj.AddComponent<LineRenderer>();
            mainLr.startWidth = beamWidth;
            mainLr.endWidth = beamWidth * 0.9f;
            mainLr.positionCount = 2;
            mainLr.SetPosition(0, gunPos);
            mainLr.SetPosition(1, endPoint);
            mainLr.material = new Material(Shader.Find("Sprites/Default"));
            mainLr.startColor = beamColor;
            mainLr.endColor = new Color(beamColor.r, beamColor.g, beamColor.b, beamColor.a * 0.7f);
            mainLr.sortingOrder = 10;

            // Bright white core
            var coreObj = new GameObject("LaserCore");
            coreObj.transform.SetParent(beamObj.transform);
            var coreLr = coreObj.AddComponent<LineRenderer>();
            coreLr.startWidth = beamWidth * 0.35f;
            coreLr.endWidth = beamWidth * 0.25f;
            coreLr.positionCount = 2;
            coreLr.SetPosition(0, gunPos);
            coreLr.SetPosition(1, endPoint);
            coreLr.material = new Material(Shader.Find("Sprites/Default"));
            coreLr.startColor = beamCoreColor;
            coreLr.endColor = new Color(1f, 1f, 1f, 0.8f);
            coreLr.sortingOrder = 11;

            var fader = beamObj.AddComponent<LaserFader>();
            fader.duration = beamDuration;
            fader.glowRenderer = glowLr;
            fader.outerRenderer = mainLr;
            fader.innerRenderer = coreLr;
        }

        void OnDestroy()
        {
            if (gun != null)
            {
                gun.ShootPojectileAction -= OnShoot;
            }
        }
    }

    public class LaserFader : MonoBehaviour
    {
        public float duration = 0.3f;
        public LineRenderer glowRenderer;
        public LineRenderer outerRenderer;
        public LineRenderer innerRenderer;

        private float timer;

        void Start()
        {
            timer = duration;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            float t = timer / duration;

            if (glowRenderer != null)
            {
                Color c = glowRenderer.startColor;
                glowRenderer.startColor = new Color(c.r, c.g, c.b, t * 0.15f);
                glowRenderer.endColor = new Color(c.r, c.g, c.b, t * 0.1f);
            }

            if (outerRenderer != null)
            {
                Color c = outerRenderer.startColor;
                outerRenderer.startColor = new Color(c.r, c.g, c.b, t * 0.9f);
                outerRenderer.endColor = new Color(c.r, c.g, c.b, t * 0.6f);
            }

            if (innerRenderer != null)
            {
                innerRenderer.startColor = new Color(1f, 1f, 1f, t);
                innerRenderer.endColor = new Color(1f, 1f, 1f, t * 0.8f);
            }
        }
    }
}
