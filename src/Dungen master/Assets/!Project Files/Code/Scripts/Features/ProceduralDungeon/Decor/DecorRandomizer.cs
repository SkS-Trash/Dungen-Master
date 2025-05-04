using System;
using System.Collections.Generic;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public class DecorRandomizer
    {
        private readonly Random _random;
        private readonly DecorGeneratorConfig _config;

        public DecorRandomizer(Random random, DecorGeneratorConfig config)
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

        private DecorType GetWeightedDecor(RoomType roomType)
        {
            var weights = _config.RoomProfiles[roomType].DecorWeights;
            var decor = WeightedRandomizer.GetRandom(weights, _random);
            return decor;
        }
    }
}