using ProceduralDungeon.Data;
using ProceduralDungeon.Data.Configs;

namespace ProceduralDungeon
{
    public class DecorRandomizer
    {
        private readonly Random _random;
        private readonly DecorConfig _config;

        public DecorRandomizer(Random random, DecorConfig config)
        {
            _random = random;
            _config = config;
        }

        public DecorType SelectDecorType(RoomType roomType, List<DecorType> specialObjects)
        {
            if (_random.NextDouble() < 0.3 &&
                specialObjects.Count > 0)
                return specialObjects[_random.Next(specialObjects.Count)];

            return GetWeightedDecor(roomType);
        }

        public DecorType GetWeightedDecor(RoomType roomType)
        {
            var weights = _config.RoomProfiles[roomType.ToString()].DecorWeights;
            var decor = WeightedRandomizer.GetRandom(weights, _random);
            if (decor == null) return DecorType.None;
            if (!Enum.TryParse(decor, out DecorType decorType))
                throw new ArgumentException($"Invalid decor type: {decor}");
            return decorType;
        }
    }
}