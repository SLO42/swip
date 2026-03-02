using System.Collections;
using ClassesManagerReborn;

namespace SWIP.Classes
{
    public class ASourFruitClass : ClassHandler
    {
        internal static string name = "ASourFruit";

        private static readonly string[] gatedCards = new string[]
        {
            "Rubber Bullets",
            "Sour Punch",
            "Fruit Salad",
            "Sour Patch",
            "Citric Acid",
            "One Punch",
            "Pinball",
            "No U",
            "Snowball Effect",
            "Agent Orange",
            "Zyklon B",
            "Chain Reaction",
            "Bounce House",
            "Scorched Earth",
            "Ricochet Rush",
            "Nuclear Option",
            "Cluster Bomb",
            "Seismic Impact",
            "Volatile Payload",
            "Aftershock",
            "Singularity",
            "Perpetual Motion"
        };

        public override IEnumerator Init()
        {
            while (!CardRegistry.TryGet("ASourFruit Entry", out _))
            {
                yield return null;
            }

            var entryCard = CardRegistry.Get("ASourFruit Entry");
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
