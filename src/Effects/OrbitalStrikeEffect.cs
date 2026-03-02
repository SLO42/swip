using UnityEngine;
using System.Collections;

namespace SWIP.Effects
{
    public class OrbitalStrikeEffect : MonoBehaviour
    {
        public int explosionCount = 3;
        public float explosionDamage = 55f;
        public float explosionRange = 4f;
        public float explosionForce = 2000f;
        public float delayBetween = 0.2f;
        public float strikeHeight = 20f;
        public bool triggerOnBlock = true;
        public bool triggerOnKill = false;

        private Player player;
        private Block block;

        void Start()
        {
            player = GetComponentInParent<Player>();
            if (player == null) return;

            if (triggerOnBlock)
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

            Player closest = FindClosestEnemy();
            if (closest == null) return;

            StartCoroutine(StrikeCoroutine(closest.transform.position));
        }

        public void TriggerStrike(Vector3 position)
        {
            StartCoroutine(StrikeCoroutine(position));
        }

        private IEnumerator StrikeCoroutine(Vector3 targetPos)
        {
            for (int i = 0; i < explosionCount; i++)
            {
                // Small random offset around the target, not above them
                Vector3 offset = new Vector3(
                    UnityEngine.Random.Range(-2f, 2f),
                    UnityEngine.Random.Range(-0.5f, 1.5f),
                    0f
                );

                var strikeObj = new GameObject("OrbitalStrike");
                strikeObj.transform.position = targetPos + offset;

                var exp = strikeObj.AddComponent<Explosion>();
                exp.auto = true;
                exp.damage = explosionDamage;
                exp.range = explosionRange;
                exp.force = explosionForce;

                yield return new WaitForSeconds(delayBetween);
            }
        }

        private Player FindClosestEnemy()
        {
            Player closest = null;
            float closestDist = float.MaxValue;

            foreach (var p in PlayerManager.instance.players)
            {
                if (p == player) continue;
                if (p.data.dead) continue;
                if (p.teamID == player.teamID) continue;

                float dist = Vector2.Distance(player.transform.position, p.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = p;
                }
            }
            return closest;
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
