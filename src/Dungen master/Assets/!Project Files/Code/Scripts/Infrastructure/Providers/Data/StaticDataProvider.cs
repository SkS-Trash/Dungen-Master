using ProceduralDungeon;
using Progress;
using UnityEngine;
using static DataPaths;

namespace Infrastructure.Providers.Data
{
    /// <summary>
    /// Поставщик статических данных.
    /// </summary>
    public class StaticDataProvider : IStaticDataProvider
    {
        public ProgressGameDataHolder GetProgressGameDataHolder()
        {
            return Resources.Load<ProgressGameDataHolder>(PROGRESS_GAME_DATA_HOLDER);
        }

        public LevelStyleConfig[] GetLevelStyleConfigs()
        {
            return Resources.LoadAll<LevelStyleConfig>(LEVEL_STYLE_CONFIGS_PATH);
        }
    }
}