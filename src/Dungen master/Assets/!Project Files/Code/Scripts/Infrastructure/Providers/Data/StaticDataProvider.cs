using GameEventObserver;
using ProceduralDungeon.Data.Configs;
using Progress;
using UnityEngine;
using static DataPaths;

namespace Providers.Data
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

        public GameEventObserverCollection GetGameEventObserverCollection()
        {
            return Resources.Load<GameEventObserverCollection>(GAME_EVENT_OBSERVER_COLLECTION);
        }
        
        public BaseGeneratorConfig GetBaseGeneratorConfig()
        {
            return Resources.Load<BaseGeneratorConfig>(BASE_GENERATOR_CONFIG);
        }
    }
}