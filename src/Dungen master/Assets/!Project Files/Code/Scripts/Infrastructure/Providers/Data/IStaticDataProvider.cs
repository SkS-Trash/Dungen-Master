using GameEventObserver;
using ProceduralDungeon.Data;
using Progress;

namespace Providers.Data
{
    /// <summary>
    /// Интерфейс для предоставления статических данных.
    /// </summary>
    public interface IStaticDataProvider
    {
        ProgressGameDataHolder GetProgressGameDataHolder();
        LevelStyleConfig[] GetLevelStyleConfigs();
        GameEventObserverCollection GetGameEventObserverCollection();
    }
}