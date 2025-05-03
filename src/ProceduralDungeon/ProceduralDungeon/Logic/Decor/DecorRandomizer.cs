using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class DecorRandomizer
    {
        private readonly Random _random;
        public DecorRandomizer(Random random)
        {
            _random = random;
        }

        public DecorType SelectDecorType(RoomType roomType, List<DecorType> specialObjects)
        {
            if (_random.NextDouble() < 0.3 && specialObjects.Count > 0)
            {
                return specialObjects[_random.Next(specialObjects.Count)];
            }
            return GetWeightedDecor(roomType);
        }

        public DecorType GetWeightedDecor(RoomType roomType)
        {
            var weights = new Dictionary<DecorType, int>
            {
                [DecorType.None] = 0,
                [DecorType.Chest] = roomType == RoomType.Trap ? 5 : 15,
                [DecorType.Barrel] = 30,
                [DecorType.Column] = 20,
                [DecorType.Campfire] = 10
            };
            return WeightedRandomizer.GetRandom(weights, _random);
        }
    }
} 