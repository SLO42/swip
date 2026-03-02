using UnityEngine;

namespace SWIP.Effects
{
    public class SizeLockEffect : MonoBehaviour
    {
        private float lockedSize;
        private CharacterStatModifiers stats;

        void Start()
        {
            var player = GetComponent<Player>();
            if (player != null)
            {
                stats = player.GetComponent<CharacterStatModifiers>();
                if (stats != null)
                {
                    lockedSize = stats.sizeMultiplier;
                }
            }
        }

        void LateUpdate()
        {
            if (stats != null && stats.sizeMultiplier > lockedSize)
            {
                stats.sizeMultiplier = lockedSize;
            }
        }
    }
}
