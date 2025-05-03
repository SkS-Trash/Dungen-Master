using ProceduralDungeon.Data.Configs;

namespace ProceduralDungeon.Data.Repositories
{
    public static class MapConfigRepository
    {
        public static MapGeneratorConfig GetTestConfig() => new()
        {
            Width = 50,
            Height = 40,
            RoomCount = 10,
            RoomMinSize = 4,
            RoomMaxSize = 10,
            GenerationMode = "Rectangular"
        };
    }
}