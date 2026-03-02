using System.Collections.Generic;
using UnityEngine;

namespace SWIP
{
    public static class CardRegistry
    {
        private static readonly Dictionary<string, CardInfo> cards = new Dictionary<string, CardInfo>();

        public static void Register(string title, CardInfo cardInfo)
        {
            cards[title] = cardInfo;
        }

        public static CardInfo Get(string title)
        {
            cards.TryGetValue(title, out var info);
            return info;
        }

        public static bool TryGet(string title, out CardInfo info)
        {
            return cards.TryGetValue(title, out info);
        }

        public static IReadOnlyDictionary<string, CardInfo> All => cards;
    }
}
