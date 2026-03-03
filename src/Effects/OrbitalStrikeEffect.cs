using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Block-triggered red laser from above targeting closest enemy.
    /// Penetrates players and destructables, stops on solid terrain.
    /// </summary>
    public class SatelliteLaserEffect : MonoBehaviour
    {
        public float laserDamage = 70f;
        public float laserWidth = 0.8f;

        private Player player;
        private Block block;
        private bool blockHooked;

        void Start()
        {
            player = GetComponentInParent<Player>();
            if (player != null) TryHookBlock();
        }

        void Update()
        {
            if (!blockHooked) TryHookBlock();
        }

        private void TryHookBlock()
        {
            if (player == null) return;

            Block foundBlock = player.GetComponent<Block>();
            if (foundBlock == null)
                foundBlock = player.GetComponentInChildren<Block>();

            if (foundBlock != null)
            {
                block = foundBlock;
                block.BlockAction += OnBlock;
                blockHooked = true;
            }
        }

        private void OnBlock(BlockTrigger.BlockTriggerType triggerType)
        {
            if (player == null) return;

            // Fire on the owner's position — protects them from above
            FireLaser(player.transform.position.x);
        }

        private void FireLaser(float targetX)
        {
            float screenTop = 30f;
            if (Camera.main != null)
            {
                screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y + 5f;
            }

            Vector2 origin = new Vector2(targetX, screenTop);
            float beamEndY = screenTop;

            // Raycast downward — hits everything along the path
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, screenTop * 2f);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            bool stopped = false;
            foreach (var hit in hits)
            {
                if (stopped) break;

                // Skip projectiles
                if (hit.collider.GetComponent<ProjectileHit>() != null) continue;
                if (hit.collider.GetComponentInParent<ProjectileHit>() != null) continue;

                // Skip owner
                var hitPlayer = hit.collider.GetComponentInParent<Player>();
                if (hitPlayer != null)
                {
                    if (hitPlayer == player || hitPlayer.teamID == player.teamID) continue;

                    if (!hitPlayer.data.dead)
                    {
                        hitPlayer.data.healthHandler.TakeDamage(Vector2.down * laserDamage, hit.point);
                    }
                    continue; // laser passes through players
                }

                // Destructable: damage and continue through
                var damagable = hit.collider.GetComponent<Damagable>();
                if (damagable != null)
                {
                    damagable.CallTakeDamage(Vector2.down * laserDamage * 2f, hit.point);
                    continue;
                }

                // Solid terrain: stop here
                beamEndY = hit.point.y;
                stopped = true;
            }

            if (!stopped)
            {
                float screenBottom = -30f;
                if (Camera.main != null)
                    screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).y - 5f;
                beamEndY = screenBottom;
            }

            // Spawn visual
            var beamObj = new GameObject("SatelliteLaser");
            beamObj.transform.position = new Vector3(targetX, 0f, 0f);
            var visual = beamObj.AddComponent<SatelliteLaserVisual>();
            visual.topY = screenTop;
            visual.bottomY = beamEndY;
            visual.laserWidth = laserWidth;
            visual.targetX = targetX;
        }

        void OnDestroy()
        {
            if (block != null)
            {
                block.BlockAction -= OnBlock;
            }
        }
    }

    /// <summary>
    /// Visual for satellite laser — thin red beam that flashes and fades quickly.
    /// </summary>
    public class SatelliteLaserVisual : MonoBehaviour
    {
        public float topY;
        public float bottomY;
        public float laserWidth = 0.8f;
        public float targetX;
        public float duration = 0.15f;

        private float timer;
        private LineRenderer beam;
        private LineRenderer glow;

        void Start()
        {
            // Core beam — bright red
            beam = gameObject.AddComponent<LineRenderer>();
            beam.useWorldSpace = true;
            beam.startWidth = laserWidth;
            beam.endWidth = laserWidth * 0.6f;
            beam.positionCount = 2;
            beam.SetPosition(0, new Vector3(targetX, topY, 0f));
            beam.SetPosition(1, new Vector3(targetX, bottomY, 0f));
            beam.material = new Material(Shader.Find("Sprites/Default"));
            beam.startColor = new Color(1f, 0.15f, 0.05f, 1f);
            beam.endColor = new Color(0.8f, 0.05f, 0f, 0.8f);
            beam.sortingOrder = 12;

            // Outer glow — red haze
            var glowObj = new GameObject("Glow");
            glowObj.transform.SetParent(transform);
            glow = glowObj.AddComponent<LineRenderer>();
            glow.useWorldSpace = true;
            glow.startWidth = laserWidth * 2f;
            glow.endWidth = laserWidth * 1.5f;
            glow.positionCount = 2;
            glow.SetPosition(0, new Vector3(targetX, topY, 0f));
            glow.SetPosition(1, new Vector3(targetX, bottomY, 0f));
            glow.material = new Material(Shader.Find("Sprites/Default"));
            glow.startColor = new Color(1f, 0.2f, 0.1f, 0.25f);
            glow.endColor = new Color(0.8f, 0.1f, 0f, 0.1f);
            glow.sortingOrder = 11;
        }

        void Update()
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            if (t >= 1f)
            {
                Destroy(gameObject);
                return;
            }

            float alpha = 1f - t;

            beam.startColor = new Color(1f, 0.15f, 0.05f, alpha);
            beam.endColor = new Color(0.8f, 0.05f, 0f, 0.8f * alpha);

            glow.startColor = new Color(1f, 0.2f, 0.1f, 0.25f * alpha);
            glow.endColor = new Color(0.8f, 0.1f, 0f, 0.1f * alpha);
        }
    }
}
