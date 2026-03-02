using System.Collections;
using ClassesManagerReborn;

namespace SWIP.Classes
{
    public class SaucyEnchiladasClass : ClassHandler
    {
        internal static string name = "SaucyEnchiladas";

        private static readonly string[] gatedCards = new string[]
        {
            "Smoke Bomb",
            "Salsa Verde",
            "Corrosive Spray",
            "Noxious Fumes",
            "Acid Rain",
            "Gas Leak",
            "Toxic Relationship",
            "Healing Mist",
            "Extra Spicy",
            "Second Wind",
            "Miasma",
            "Special Sauce",
            "The Enchilada Treatment",
            "Hot Ones Challenge",
            "Biohazard",
            "Plague Doctor",
            "Chemical Warfare",
            "Life Drain",
            "Contagion",
            "Dead Zone",
            "Pandemic",
            "Eternal Mist"
        };

        public override IEnumerator Init()
        {
            if (!Plugin.UseClasses) yield break;

            while (!CardRegistry.TryGet("SaucyEnchiladas Entry", out _))
            {
                yield return null;
            }

            var entryCard = CardRegistry.Get("SaucyEnchiladas Entry");
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
