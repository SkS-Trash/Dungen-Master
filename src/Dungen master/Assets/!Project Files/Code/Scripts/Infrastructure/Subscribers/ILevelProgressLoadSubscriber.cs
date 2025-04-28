using Progress;
using Subscribers.EventBusSystem;

namespace Services.Progress
{
    /// <summary>
    /// Интерфейс для подписчиков на загрузку прогресса уровня.
    /// </summary>
    public interface ILevelProgressLoadSubscriber : IGlobalSubscriber
    {
        /// <summary>
        /// Загрузить прогресс уровня.
        /// </summary>
        /// <param name="progress"> Прогресс уровня.</param>
        void OnProgressLoaded(LevelSaveData progress);
    }
}