using ProceduralDungeon.Data;
using ProceduralDungeon.Data.Configs;

namespace ProceduralDungeon
{
    public class DecorProfileProvider
    {
        private readonly DecorConfig _config;

        public DecorProfileProvider(DecorConfig config)
        {
            _config = config;
        }

        public (int baseDensity, List<DecorType> specialObjects) GetRoomDecorProfile(RoomType type)
        {
            _config.RoomProfiles.TryGetValue(type.ToString(), out var profile);

            if (profile == null)
                throw new ArgumentException($"No decor profile found for room type: {type}");

            var baseDensity = profile.BaseDensity;

            var specialObjects = (from decor in profile.DecorWeights
                where decor.Value > 0
                select Enum.Parse<DecorType>(decor.Key)).ToList();

            if (specialObjects.Count == 0)
                throw new ArgumentException($"No special objects found for room type: {type}");

            return (baseDensity, specialObjects);
        }
    }
}