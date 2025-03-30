using ProceduralDungeon;

namespace Core.Project.Dungeon
{
    public class DungeonGenerationData
    {
        public int Width;
        public int Height;

        public int RoomCount;
        public int RoomMinSize;
        public int RoomMaxSize;

        public TileType[,] MapLayer;
        public DecorType[,] DecorLayer;
        public EnemyType[,] EnemyLayer;
    }
}