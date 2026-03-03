using System.Collections;
using UnityEngine;

namespace SWIP.Effects
{
    public class TortillaWrapEffect : MonoBehaviour
    {
        public float wrapDuration = 3f;

        private Player player;
        private Block block;
        private bool wrapping;

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
            if (player == null || wrapping) return;
            StartCoroutine(WrapCoroutine());
        }

        private IEnumerator WrapCoroutine()
        {
            wrapping = true;

            // Store and disable movement
            var characterStats = player.data.stats;
            float origSpeed = characterStats.movementSpeed;
            characterStats.movementSpeed = 0f;

            // Heal to max for near-invulnerability
            float maxHp = player.data.maxHealth;
            player.data.healthHandler.Heal(maxHp);

            // Visual shell — brown circle around player
            var shellObj = new GameObject("TortillaShell");
            shellObj.transform.SetParent(player.transform);
            shellObj.transform.localPosition = Vector3.zero;

            var lr = shellObj.AddComponent<LineRenderer>();
            lr.useWorldSpace = false;
            lr.startWidth = 0.15f;
            lr.endWidth = 0.15f;
            lr.loop = true;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = new Color(0.8f, 0.65f, 0.3f, 0.8f);
            lr.endColor = new Color(0.7f, 0.55f, 0.2f, 0.8f);
            lr.sortingOrder = 10;

            int segments = 16;
            lr.positionCount = segments;
            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                lr.SetPosition(i, new Vector3(Mathf.Cos(angle) * 1.5f, Mathf.Sin(angle) * 1.5f, 0f));
            }

            // Heal continuously during wrap
            float elapsed = 0f;
            while (elapsed < wrapDuration)
            {
                elapsed += Time.deltaTime;
                if (player != null && !player.data.dead)
                {
                    player.data.healthHandler.Heal(maxHp * Time.deltaTime);
                }
                yield return null;
            }

            // Restore
            characterStats.movementSpeed = origSpeed;
            wrapping = false;
            if (shellObj != null)
            {
                Destroy(shellObj);
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
}
