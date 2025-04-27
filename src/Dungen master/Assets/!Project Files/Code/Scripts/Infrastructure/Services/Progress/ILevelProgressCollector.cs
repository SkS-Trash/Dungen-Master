using Progress;

namespace Services.Progress
{
    /// <summary>
    /// Интерфейс для подписчиков на сохранение прогресса уровня.
    /// </summary>
    public interface ILevelProgressCollector
    {
        /// <summary>
        /// Собрать прогресс уровня.
        /// </summary>
        void Collect(LevelSaveData target);
    }
}