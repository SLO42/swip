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

        void Start()
        {
            timer = duration;
            growTimer = 0f;
            CreateVisual();
        }

        private void CreateVisual()
        {
            var particleObj = new GameObject("ZoneSmoke");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = Vector3.zero;

            particles = particleObj.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // Main module
            var main = particles.main;
            main.loop = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(1.0f, 1.8f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 0.4f);
            main.startSize = new ParticleSystem.MinMaxCurve(radius * 0.4f, radius * 0.8f);
            main.startColor = outerColor;
            main.maxParticles = 40;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = -0.03f;
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);

            // Emission
            var emission = particles.emission;
            emission.rateOverTime = Mathf.Max(10f, radius * 5f);

            // Shape — circle, starts at 0 and grows
            shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;
            shape.radiusThickness = 1f;

            // Size over lifetime — grow then shrink for billowing smoke
            var sol = particles.sizeOverLifetime;
            sol.enabled = true;
            sol.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, 0.3f),
                new Keyframe(0.25f, 1f),
                new Keyframe(0.7f, 0.9f),
                new Keyframe(1f, 0.2f)
            ));

            // Color over lifetime — fade in, hold, fade out
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
                    new GradientAlphaKey(0.8f, 0.15f),
                    new GradientAlphaKey(0.7f, 0.6f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            col.color = gradient;

            // Rotation over lifetime — slow spin for organic look
            var rot = particles.rotationOverLifetime;
            rot.enabled = true;
            rot.z = new ParticleSystem.MinMaxCurve(-0.3f, 0.3f);

            // Renderer
            var renderer = particleObj.GetComponent<ParticleSystemRenderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));
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
