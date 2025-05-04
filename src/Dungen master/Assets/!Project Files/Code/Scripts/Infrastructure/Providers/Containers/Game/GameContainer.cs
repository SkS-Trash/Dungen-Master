using ProceduralDungeon.Data;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Configs.Enemy;
using ProceduralDungeon.Data.Configs.Map;
using ProceduralDungeon.Data.Types;
using UnityEngine;

namespace Providers.Containers.Game
{
    public class GameContainer : IGameContainer
    {
        #region DungeonGenerationData

        public LevelStyleConfig LevelStyleConfig { get; set; }

        public int Seed { get; set; }

        public MapGeneratorConfig MapGeneratorConfig { get; set; }
        public DecorGeneratorConfig DecorConfig { get; set; }
        public EnemyGeneratorConfig EnemyConfig { get; set; }

        public TileType[,] MapLayer { get; set; }
        public DecorType[,] DecorLayer { get; set; }
        public EnemyType[,] EnemyLayer { get; set; }

        #endregion

        #region PlayerData

        public Transform PlayerSpawnPoint { get; set; }

        public Transform PlayerTransform { get; set; }

        #endregion
    }
}