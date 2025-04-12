using ProceduralDungeon;

namespace Providers.Containers.Game
{
    public interface IGameContainer :
        IDungeonGenerationData
    {
    }

    public class GameContainer : IGameContainer
    {
        #region IDungeonGenerationData

        public LevelStyleConfig LevelStyleConfig { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        
        public int RoomCount { get; set; }
        public int RoomMinSize { get; set; }
        public int RoomMaxSize { get; set; }
        
        public TileType[,] MapLayer { get; set; }
        public DecorType[,] DecorLayer { get; set; }
        public EnemyType[,] EnemyLayer { get; set; }

        #endregion
    }
}