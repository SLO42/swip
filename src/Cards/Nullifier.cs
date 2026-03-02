using System.Linq;
using SWIP.Effects;
using UnboundLib.Cards;
using UnityEngine;

namespace SWIP.Cards
{
    public class NullifierEffect : MonoBehaviour
    {
        private Player player;
        private GameObject zoneObj;
        private const float radius = 5f;

        private void Start()
        {
            player = GetComponentInParent<Player>();

            zoneObj = new GameObject("NullifierAura");
            zoneObj.transform.SetParent(player.transform);
            zoneObj.transform.localPosition = Vector3.zero;

            var zone = zoneObj.AddComponent<ZoneBehaviour>();
            zone.owner = player;
            zone.followTarget = player.transform;
            zone.duration = float.MaxValue;
            zone.radius = radius;
            zone.damagePerSecond = 0f;
            zone.healPerSecond = 0f;
            zone.slowAmount = 0f;
            zone.affectsOwner = true;
            zone.outerColor = new Color(1f, 1f, 1f, 0.3f);
            zone.innerColor = new Color(1f, 1f, 1f, 0.1f);
        }

        private void FixedUpdate()
        {
            if (player == null) return;

            foreach (var p in PlayerManager.instance.players)
            {
                if (Vector3.Distance(p.transform.position, player.transform.position) <= radius)
                {
                    var burn = p.GetComponent<BurnDamageEffect>();
                    if (burn != null) Object.Destroy(burn);

                    var freeze = p.GetComponent<FreezeDamageEffect>();
                    if (freeze != null) Object.Destroy(freeze);
                }
            }
        }

        private void OnDestroy()
        {
            if (zoneObj != null)
            {
                Object.Destroy(zoneObj);
            }
        }
    }

    public class Nullifier : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<NullifierEffect>();
            gun.damage *= 0.7f;
            data.maxHealth *= 1.2f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var effect = player.gameObject.GetComponent<NullifierEffect>();
            if (effect != null) Object.Destroy(effect);
            gun.damage /= 0.7f;
            data.maxHealth /= 1.2f;
        }

        protected override string GetTitle() => "Nullifier";
        protected override string GetDescription() => "A void of nothingness. No fire. No ice. No poison. Just silence.";

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Negates Effects",
                    amount = "Radius 5",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "HP",
                    amount = "+20%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity() => RarityLib.Utils.RarityUtils.GetRarity("Legendary");
        protected override GameObject GetCardArt() => null;
        protected override CardThemeColor.CardThemeColorType GetTheme() => CardThemeColor.CardThemeColorType.TechWhite;
        public override string GetModName() => "SWIP";
    }
}
