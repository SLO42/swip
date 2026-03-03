using UnityEngine;

namespace SWIP.Effects
{
    public class NachosOnBlockEffect : MonoBehaviour
    {
        public int nachoCount = 5;
        public float nachoDamage = 10f;
        public float nachoHeal = 5f;
        public float nachoSpeed = 8f;
        public float nachoLifetime = 3f;

        private Player player;
        private Block block;

        void Start()
        {
            player = GetComponentInParent<Player>();
            if (player != null)
            {
                block = player.GetComponent<Block>();
                if (block != null)
                {
                    block.BlockAction += OnBlock;
                }
            }
        }

        private void OnBlock(BlockTrigger.BlockTriggerType triggerType)
        {
            if (player == null) return;

            for (int i = 0; i < nachoCount; i++)
            {
                float angle = (360f / nachoCount) * i;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

                var nachoObj = new GameObject("Nacho");
                nachoObj.transform.position = player.transform.position;

                var rb = nachoObj.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0.5f;
                rb.velocity = dir * nachoSpeed;
                rb.mass = 0.1f;

                var col = nachoObj.AddComponent<CircleCollider2D>();
                col.radius = 0.15f;
                col.isTrigger = true;

                // Yellow triangle visual
                var lr = nachoObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = false;
                lr.startWidth = 0.08f;
                lr.endWidth = 0.08f;
                lr.positionCount = 4;
                lr.SetPosition(0, new Vector3(-0.1f, -0.08f, 0f));
                lr.SetPosition(1, new Vector3(0.1f, -0.08f, 0f));
                lr.SetPosition(2, new Vector3(0f, 0.12f, 0f));
                lr.SetPosition(3, new Vector3(-0.1f, -0.08f, 0f));
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = new Color(1f, 0.85f, 0.2f, 1f);
                lr.endColor = new Color(0.9f, 0.7f, 0.1f, 1f);
                lr.sortingOrder = 5;
                lr.loop = true;

                var nacho = nachoObj.AddComponent<NachoProjectile>();
                nacho.owner = player;
                nacho.damage = nachoDamage;
                nacho.healAmount = nachoHeal;
                nacho.lifetime = nachoLifetime;
            }
        }

        void OnDestroy()
        {
            if (block != null)
            {
                block.BlockAction -= OnBlock;
            }
        }
    }

    public class NachoProjectile : MonoBehaviour
    {
        public Player owner;
        public float damage = 10f;
        public float healAmount = 5f;
        public float lifetime = 3f;

        private float timer;
        private bool used;

        void Start()
        {
            timer = lifetime;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (used) return;

            var player = other.GetComponentInParent<Player>();
            if (player == null || player.data.dead) return;

            if (owner != null && player.teamID == owner.teamID)
            {
                // Heal ally
                player.data.healthHandler.Heal(healAmount);
            }
            else
            {
                // Damage enemy
                player.data.healthHandler.TakeDamage(Vector2.up * damage, transform.position);
            }

            used = true;
            Destroy(gameObject);
        }
    }
}
