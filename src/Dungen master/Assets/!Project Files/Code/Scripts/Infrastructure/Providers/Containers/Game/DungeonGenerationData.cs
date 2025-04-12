using ProceduralDungeon;

namespace Providers.Containers.Game
{
    public interface IDungeonGenerationData
    {
        LevelStyleConfig LevelStyleConfig { get; set; }

        int Width { get; set; }
        int Height { get; set; }

        int RoomCount { get; set; }
        int RoomMinSize { get; set; }
        int RoomMaxSize { get; set; }

        TileType[,] MapLayer { get; set; }
        DecorType[,] DecorLayer { get; set; }
        EnemyType[,] EnemyLayer { get; set; }
    }
}