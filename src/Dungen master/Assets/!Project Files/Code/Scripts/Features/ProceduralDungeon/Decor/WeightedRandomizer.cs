using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon.Decor
{
    public static class WeightedRandomizer
    {
        public static T GetRandom<T>(Dictionary<T, int> weights)
        {
            var total = weights.Values.Sum();
            var random = new Random().Next(total);

            foreach (var kvp in weights)
            {
                if (random < kvp.Value) return kvp.Key;
                random -= kvp.Value;
            }

            return default;
        }
    }
}