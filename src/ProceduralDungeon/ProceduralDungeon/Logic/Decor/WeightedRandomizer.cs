namespace ProceduralDungeon
{
    public static class WeightedRandomizer
    {
        public static T GetRandom<T>(Dictionary<T, int> weights, Random random)
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