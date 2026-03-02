using UnityEngine;

namespace SWIP.Effects
{
    public class ZoneBehaviour : MonoBehaviour
    {
        // Core
        public float radius = 3f;
        public float duration = 5f;
        public Player owner;
        public bool affectsOwner;

        // Visual
        public Color outerColor = new Color(0.2f, 0.8f, 0.2f, 0.5f);
        public Color innerColor = new Color(0.3f, 0.9f, 0.3f, 0.35f);

        // Effects (all per-second, multiple can be active simultaneously)
        public float damagePerSecond;
        public float healPerSecond;
        public float slowAmount;

        // Follow target (for player-attached auras)
        public Transform followTarget;

        private float timer;
        private ParticleSystem particles;
        private ParticleSystem.ShapeModule shape;
        private float growTimer;
        private const float GROW_DURATION = 0.4f;

        private static Texture2D _circleTexture;

        void Start()
        {
            timer = duration;
            growTimer = 0f;
            CreateVisual();
        }

        /// <summary>
        /// Creates a soft circular gradient texture for gas particles.
        /// Cached statically so it's only generated once across all zones.
        /// </summary>
        private static Texture2D GetCircleTexture()
        {
            if (_circleTexture != null) return _circleTexture;

            const int size = 64;
            _circleTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            float center = size / 2f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy) / center;

                    // Soft radial falloff — dense core that fades to wispy edges
                    float alpha = Mathf.Clamp01(1f - dist);
                    alpha = alpha * alpha; // quadratic falloff for softer edges
                    _circleTexture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            _circleTexture.Apply();
            _circleTexture.wrapMode = TextureWrapMode.Clamp;
            return _circleTexture;
        }

        private void CreateVisual()
        {
            var particleObj = new GameObject("ZoneSmoke");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = Vector3.zero;

            particles = particleObj.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // Main module — slower, longer-lived particles for a lingering gas feel
            var main = particles.main;
            main.loop = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(1.5f, 2.8f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
            main.startSize = new ParticleSystem.MinMaxCurve(radius * 0.5f, radius * 1.0f);
            main.startColor = outerColor;
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = -0.02f;
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);

            // Emission — denser for thicker gas
            var emission = particles.emission;
            emission.rateOverTime = Mathf.Max(12f, radius * 6f);

            // Shape — circle, starts at 0 and grows
            shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;
            shape.radiusThickness = 1f;

            // Noise — adds turbulence for swirling, organic gas movement
            var noise = particles.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
            noise.frequency = 0.5f;
            noise.scrollSpeed = 0.3f;
            noise.damping = true;
            noise.octaveCount = 2;

            // Size over lifetime — slow bloom then dissolve
            var sol = particles.sizeOverLifetime;
            sol.enabled = true;
            sol.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, 0.2f),
                new Keyframe(0.15f, 0.7f),
                new Keyframe(0.5f, 1f),
                new Keyframe(0.85f, 0.8f),
                new Keyframe(1f, 0.1f)
            ));

            // Color over lifetime — ghostly fade in/out with lingering mid-section
            var col = particles.colorOverLifetime;
            col.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(0.6f, 0.1f),
                    new GradientAlphaKey(0.5f, 0.5f),
                    new GradientAlphaKey(0.3f, 0.8f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            col.color = gradient;

            // Rotation over lifetime — gentle drift for wispy gas
            var rot = particles.rotationOverLifetime;
            rot.enabled = true;
            rot.z = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);

            // Renderer — circular soft particle texture
            var renderer = particleObj.GetComponent<ParticleSystemRenderer>();
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.mainTexture = GetCircleTexture();
            renderer.material = mat;
            renderer.sortingOrder = 5;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;

            particles.Play();
        }

        void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            // Grow effect — expand emission radius from 0 to full over GROW_DURATION
            if (growTimer < GROW_DURATION)
            {
                growTimer += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(growTimer / GROW_DURATION);
                // Ease out for a natural expansion
                float eased = 1f - (1f - t) * (1f - t);
                shape.radius = radius * 0.7f * eased;
            }

            // Fade out as zone expires (reduce emission + alpha)
            if (duration < float.MaxValue && particles != null)
            {
                float life = Mathf.Clamp01(timer / duration);
                var emission = particles.emission;
                emission.rateOverTime = Mathf.Max(10f, radius * 5f) * life;

                // Tint particles toward transparent as zone fades
                var main = particles.main;
                Color c = outerColor;
                c.a = outerColor.a * Mathf.Clamp01(life * 2f);
                main.startColor = c;
            }

            // Follow target
            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }

            // Apply effects to players in radius
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (var hit in hits)
            {
                var player = hit.GetComponentInParent<Player>();
                if (player == null || player.data.dead) continue;
                if (owner != null && player == owner && !affectsOwner) continue;

                if (damagePerSecond > 0f)
                {
                    player.data.healthHandler.TakeDamage(
                        Vector2.up * damagePerSecond * Time.fixedDeltaTime,
                        transform.position
                    );
                }

                if (healPerSecond > 0f)
                {
                    player.data.healthHandler.Heal(healPerSecond * Time.fixedDeltaTime);
                }

                if (slowAmount > 0f)
                {
                    var rb = player.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity *= (1f - slowAmount);
                    }
                }
            }
        }

        void OnDestroy()
        {
            // Clean up particle system when component is removed
            var smoke = transform.Find("ZoneSmoke");
            if (smoke != null) Destroy(smoke.gameObject);
        }
    }
}
