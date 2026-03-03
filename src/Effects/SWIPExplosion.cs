using UnityEngine;

namespace SWIP.Effects
{
    /// <summary>
    /// Custom explosion that handles AoE damage, knockback, and visuals.
    /// </summary>
    public static class SWIPExplosion
    {
        // Preset color schemes: [ring, particles, flash]
        public static readonly Color[] FireColors = { new Color(1f, 0.7f, 0.2f, 1f), new Color(1f, 0.4f, 0.1f, 0.9f), new Color(1f, 0.8f, 0.3f, 0.8f) };
        public static readonly Color[] OrbitalColors = { new Color(0.4f, 0.7f, 1f, 1f), new Color(0.2f, 0.5f, 1f, 0.9f), new Color(0.6f, 0.85f, 1f, 0.8f) };
        public static readonly Color[] MissileColors = { new Color(1f, 0.3f, 0.1f, 1f), new Color(0.8f, 0.15f, 0f, 0.9f), new Color(1f, 0.5f, 0.2f, 0.8f) };

        public static void Spawn(Vector3 position, float damage, float range, float force, Player ignorePlayer = null, Color[] colors = null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, range);
            foreach (var hit in hits)
            {
                // Skip our custom projectiles — don't let explosions chain-kill missiles/sky projectiles
                if (hit.GetComponent<MissileHomingBehaviour>() != null) continue;
                if (hit.GetComponent<SkyProjectileBehaviour>() != null) continue;

                Vector2 dir = ((Vector2)hit.transform.position - (Vector2)position);
                float dist = dir.magnitude;
                if (dist < 0.1f) dist = 0.1f;
                dir = dir.normalized;
                float falloff = 1f - (dist / range) * 0.5f;

                // Damage players
                var player = hit.GetComponentInParent<Player>();
                if (player != null && !player.data.dead)
                {
                    if (ignorePlayer == null || player.teamID != ignorePlayer.teamID)
                    {
                        player.data.healthHandler.TakeDamage(dir * damage * falloff, position);
                    }
                }

                // Damage destructables (boxes, terrain pieces, etc.)
                if (player == null)
                {
                    var damagable = hit.GetComponent<Damagable>();
                    if (damagable == null) damagable = hit.GetComponentInParent<Damagable>();
                    if (damagable != null)
                    {
                        damagable.CallTakeDamage(dir * damage * falloff * 2f, position);
                    }
                }

                // Knockback via attachedRigidbody (most reliable for physics objects)
                var rb = hit.attachedRigidbody;
                if (rb != null && !rb.isKinematic)
                {
                    rb.AddForce(dir * force * falloff, ForceMode2D.Impulse);
                }
            }

            // Visual
            Color[] c = colors ?? FireColors;
            var flashObj = new GameObject("SWIPExplosion");
            flashObj.transform.position = position;
            var anim = flashObj.AddComponent<SWIPExplosionVisual>();
            anim.maxRadius = range;
            anim.ringColor = c[0];
            anim.particleColor = c[1];
            anim.flashColor = c[2];
        }
    }

    /// <summary>
    /// Vanilla-style explosion: white-hot flash, expanding ring, flying sparks, lingering smoke.
    /// </summary>
    public class SWIPExplosionVisual : MonoBehaviour
    {
        public float maxRadius = 3f;
        public Color ringColor = new Color(1f, 0.7f, 0.2f, 1f);
        public Color particleColor = new Color(1f, 0.4f, 0.1f, 0.9f);
        public Color flashColor = new Color(1f, 0.8f, 0.3f, 0.8f);

        private const float BLAST_DURATION = 0.2f;   // bright flash + ring + sparks
        private const float SMOKE_DURATION = 0.6f;    // grey smoke lingers after

        private float timer;
        private LineRenderer ring;
        private SpriteRenderer flash;

        // Sparks
        private int sparkCount;
        private Transform[] sparks;
        private SpriteRenderer[] sparkRenderers;
        private Vector2[] sparkVelocities;

        // Smoke puffs
        private int smokeCount;
        private Transform[] smokePuffs;
        private SpriteRenderer[] smokeRenderers;
        private Vector2[] smokeDrift;
        private float[] smokeStartSize;

        void Start()
        {
            // --- Expanding ring (thick, bright) ---
            ring = gameObject.AddComponent<LineRenderer>();
            ring.useWorldSpace = true;
            ring.loop = true;
            ring.widthMultiplier = 0.3f;
            ring.material = new Material(Shader.Find("Sprites/Default"));
            ring.startColor = ringColor;
            ring.endColor = ringColor;
            ring.sortingOrder = 10;
            ring.positionCount = 32;

            // --- Center flash (starts white-hot, large) ---
            var flashObj = new GameObject("Flash");
            flashObj.transform.SetParent(transform);
            flashObj.transform.localPosition = Vector3.zero;
            flash = flashObj.AddComponent<SpriteRenderer>();
            flash.sprite = GetCircleSprite();
            flash.color = new Color(1f, 1f, 1f, 1f);
            flash.sortingOrder = 11;
            flashObj.transform.localScale = Vector3.one * maxRadius;

            // --- Sparks flying outward ---
            sparkCount = Mathf.Max(10, (int)(maxRadius * 5f));
            sparks = new Transform[sparkCount];
            sparkRenderers = new SpriteRenderer[sparkCount];
            sparkVelocities = new Vector2[sparkCount];

            for (int i = 0; i < sparkCount; i++)
            {
                float angle = (float)i / sparkCount * Mathf.PI * 2f + Random.Range(-0.4f, 0.4f);
                float speed = maxRadius * Random.Range(4f, 8f);

                var pObj = new GameObject("Spark");
                pObj.transform.SetParent(transform);
                pObj.transform.localPosition = Vector3.zero;
                pObj.transform.localScale = Vector3.one * Random.Range(0.1f, 0.22f);

                var sr = pObj.AddComponent<SpriteRenderer>();
                sr.sprite = GetCircleSprite();
                sr.color = Color.Lerp(particleColor, new Color(1f, 1f, 0.9f, 1f), Random.Range(0f, 0.5f));
                sr.sortingOrder = 12;

                sparks[i] = pObj.transform;
                sparkRenderers[i] = sr;
                sparkVelocities[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
            }

            // --- Smoke puffs (grey, linger) ---
            smokeCount = Mathf.Max(4, (int)(maxRadius * 2f));
            smokePuffs = new Transform[smokeCount];
            smokeRenderers = new SpriteRenderer[smokeCount];
            smokeDrift = new Vector2[smokeCount];
            smokeStartSize = new float[smokeCount];

            for (int i = 0; i < smokeCount; i++)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float dist = Random.Range(0f, maxRadius * 0.4f);

                var sObj = new GameObject("Smoke");
                sObj.transform.SetParent(transform);
                sObj.transform.localPosition = new Vector3(
                    Mathf.Cos(angle) * dist,
                    Mathf.Sin(angle) * dist,
                    0f
                );

                float size = maxRadius * Random.Range(0.4f, 0.8f);
                smokeStartSize[i] = size;
                sObj.transform.localScale = Vector3.one * size * 0.3f;

                var sr = sObj.AddComponent<SpriteRenderer>();
                sr.sprite = GetCircleSprite();
                float grey = Random.Range(0.3f, 0.5f);
                sr.color = new Color(grey, grey, grey, 0f); // starts invisible, fades in
                sr.sortingOrder = 8;

                smokePuffs[i] = sObj.transform;
                smokeRenderers[i] = sr;
                smokeDrift[i] = new Vector2(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0.3f, 1.2f) // drift upward
                );
            }
        }

        void Update()
        {
            timer += Time.deltaTime;
            float dt = Time.deltaTime;

            float totalDuration = SMOKE_DURATION;
            if (timer >= totalDuration)
            {
                Destroy(gameObject);
                return;
            }

            float blastT = Mathf.Clamp01(timer / BLAST_DURATION);
            float smokeT = Mathf.Clamp01(timer / SMOKE_DURATION);

            // --- Ring: fast expansion, ease-out ---
            if (blastT < 1f)
            {
                float eased = 1f - (1f - blastT) * (1f - blastT);
                float r = maxRadius * eased;
                float rAlpha = 1f - blastT;

                ring.startColor = new Color(ringColor.r, ringColor.g, ringColor.b, rAlpha);
                ring.endColor = new Color(ringColor.r, ringColor.g, ringColor.b, rAlpha * 0.8f);
                ring.widthMultiplier = Mathf.Lerp(0.35f, 0.05f, blastT);

                Vector3 center = transform.position;
                for (int i = 0; i < ring.positionCount; i++)
                {
                    float angle = (float)i / ring.positionCount * Mathf.PI * 2f;
                    ring.SetPosition(i, center + new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
                }
            }
            else if (ring.enabled)
            {
                ring.enabled = false;
            }

            // --- Flash: white-hot → theme color → gone ---
            if (flash != null)
            {
                if (blastT < 1f)
                {
                    Color fc = Color.Lerp(new Color(1f, 1f, 1f, 1f), flashColor, blastT);
                    fc.a = Mathf.Max(0f, 1f - blastT * 2f);
                    flash.color = fc;
                    flash.transform.localScale = Vector3.one * maxRadius * Mathf.Lerp(1f, 0.15f, blastT);
                }
                else
                {
                    flash.color = new Color(0, 0, 0, 0);
                }
            }

            // --- Sparks: fly outward fast, drag, fade ---
            for (int i = 0; i < sparkCount; i++)
            {
                if (sparks[i] == null) continue;

                sparkVelocities[i] *= (1f - dt * 5f);
                sparks[i].localPosition += (Vector3)sparkVelocities[i] * dt;

                // Gravity pull on sparks
                sparkVelocities[i] += Vector2.down * dt * 3f;

                var sr = sparkRenderers[i];
                Color c = sr.color;
                c.a = Mathf.Max(0f, 1f - blastT * 1.8f);
                sr.color = c;
                sparks[i].localScale *= (1f - dt * 4f);
            }

            // --- Smoke: fades in as blast fades out, drifts up, grows, then fades ---
            for (int i = 0; i < smokeCount; i++)
            {
                if (smokePuffs[i] == null) continue;

                // Drift upward slowly
                smokePuffs[i].localPosition += (Vector3)smokeDrift[i] * dt;

                // Grow over time
                float growFactor = Mathf.Lerp(0.3f, 1f, smokeT);
                smokePuffs[i].localScale = Vector3.one * smokeStartSize[i] * growFactor;

                // Alpha: fade in during blast (0→0.4), then fade out during smoke phase
                var sr = smokeRenderers[i];
                Color sc = sr.color;
                float smokeAlpha;
                if (blastT < 1f)
                {
                    smokeAlpha = blastT * 0.4f; // fade in as explosion happens
                }
                else
                {
                    float fadePhase = (timer - BLAST_DURATION) / (SMOKE_DURATION - BLAST_DURATION);
                    smokeAlpha = 0.4f * (1f - fadePhase);
                }
                sc.a = Mathf.Max(0f, smokeAlpha);
                sr.color = sc;
            }
        }

        // --- Shared circle sprite ---
        private static Sprite _circleSprite;

        public static Sprite GetCircleSprite()
        {
            if (_circleSprite != null) return _circleSprite;

            int size = 32;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            float center = size / 2f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Mathf.Sqrt((x - center) * (x - center) + (y - center) * (y - center)) / center;
                    float a = Mathf.Clamp01(1f - dist);
                    a = a * a;
                    tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
                }
            }
            tex.Apply();
            _circleSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            return _circleSprite;
        }
    }
}
