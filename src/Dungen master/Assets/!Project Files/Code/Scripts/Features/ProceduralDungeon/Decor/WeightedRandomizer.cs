using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public static class WeightedRandomizer
    {
        public static T? GetRandom<T>(IDictionary<T, int> weights, Random random) where T : notnull
        {
            var total = weights.Values.Sum();
            var randomValue = random.Next(total);

            foreach (var kvp in weights)
            {
                if (randomValue < kvp.Value) return kvp.Key;
                randomValue -= kvp.Value;
            }

            return default;
        }
    }
}