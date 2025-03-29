using ProceduralDungeon;
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
    }
}