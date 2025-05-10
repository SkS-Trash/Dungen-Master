using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public class DecorProfileProvider
    {
        private readonly DecorGeneratorConfig _config;

        public DecorProfileProvider(DecorGeneratorConfig config)
        {
            _config = config;
        }

        public (float baseDensity, List<DecorType> specialObjects) GetRoomDecorProfile(RoomType type)
        {
            _config.RoomProfiles.TryGetValue(type, out var profile);

            if (profile == null)
                throw new ArgumentException($"No decor profile found for room type: {type}");

            var baseDensity = profile.BaseDensity;

            var specialObjects = (
                from decor in profile.DecorWeights
                where decor.Value > 0
                select decor.Key
            ).ToList();

            if (specialObjects.Count == 0)
                throw new ArgumentException($"No special objects found for room type: {type}");

            return (baseDensity, specialObjects);
        }
    }
}