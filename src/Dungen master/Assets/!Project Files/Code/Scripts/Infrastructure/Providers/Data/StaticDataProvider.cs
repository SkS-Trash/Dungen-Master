using UnityEngine;
using static DataPaths;

namespace Providers
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
    }
}