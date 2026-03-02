using System.Collections;
using ClassesManagerReborn;

namespace SWIP.Classes
{
    public class SynogenceClass : ClassHandler
    {
        internal static string name = "Synogence";

        private static readonly string[] gatedCards = new string[]
        {
            "Arsonist",
            "Lemon Zest",
            "Trail Blazer",
            "The Algorithm",
            "Synapse Fire",
            "Firewall",
            "Ghost Pepper",
            "Protocol Override",
            "Buffer Overflow",
            "Recursive Loop",
            "Holy Light",
            "Frostbite",
            "Core Stability",
            "Laser Precision",
            "Napalm",
            "Thermal Shock",
            "Cauterize",
            "Permafrost",
            "Glacial Armor",
            "Inferno",
            "Absolute Zero",
            "Supernova"
        };

        public override IEnumerator Init()
        {
            while (!CardRegistry.TryGet("Synogence Entry", out _))
            {
                yield return null;
            }

            var entryCard = CardRegistry.Get("Synogence Entry");
            ClassesRegistry.Register(entryCard, CardType.Entry);

            foreach (var title in gatedCards)
            {
                if (CardRegistry.TryGet(title, out var card))
                {
                    ClassesRegistry.Register(card, CardType.Card, entryCard);
                }
            }
        }
    }
}
