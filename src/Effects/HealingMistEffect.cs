using UnityEngine;

namespace SWIP.Effects
{
    public class HealingMistEffect : MonoBehaviour
    {
        public float cloudRadius = 3f;
        public float cloudDuration = 5f;
        public float healPerSecond = 15f;
        public Color outerColor = new Color(0.3f, 0.9f, 0.5f, 0.5f);
        public Color innerColor = new Color(0.5f, 1f, 0.7f, 0.35f);

        private Player player;
        private Block block;

        void Start()
        {
            player = GetComponentInParent<Player>();
            if (player == null) return;

            block = player.GetComponent<Block>();
            if (block != null)
            {
                block.BlockAction += OnBlock;
            }
        }

        private void OnBlock(BlockTrigger.BlockTriggerType triggerType)
        {
            if (player == null) return;

            var cloudObj = new GameObject("HealingMist");
            cloudObj.transform.position = player.transform.position;
            var zone = cloudObj.AddComponent<ZoneBehaviour>();
            zone.radius = cloudRadius;
            zone.duration = cloudDuration;
            zone.healPerSecond = healPerSecond;
            zone.outerColor = outerColor;
            zone.innerColor = innerColor;
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
