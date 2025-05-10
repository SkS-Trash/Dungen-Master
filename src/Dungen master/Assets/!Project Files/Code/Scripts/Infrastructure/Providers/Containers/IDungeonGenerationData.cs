using ProceduralDungeon.Data.Configs;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Configs.Enemy;
using ProceduralDungeon.Data.Configs.Map;
using ProceduralDungeon.Data.Types;

namespace Providers.Containers
{
    public interface IDungeonGenerationData
    {
        LevelStyleConfig LevelStyleConfig { get; set; }

        int Seed { get; set; }

        TileGeneratorConfig MapGeneratorConfig { get; set; }
        DecorGeneratorConfig DecorConfig { get; set; }
        EnemyGeneratorConfig EnemyConfig { get; set; }

        TileType[,] MapLayer { get; set; }
        DecorType[,] DecorLayer { get; set; }
        EnemyType[,] EnemyLayer { get; set; }
    }
}