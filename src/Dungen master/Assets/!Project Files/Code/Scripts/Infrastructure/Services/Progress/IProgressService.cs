using Progress;

namespace Services.Progress
{
    /// <summary>
    /// Сервис для работы с прогрессом игры.
    /// </summary>
    public interface IProgressService
    {
        /// <summary>
        /// Текущий прогресс игры.
        /// </summary>
        GlobalSaveData GlobalProgress { get; }

        /// <summary>
        /// Текущий прогресс уровня.
        /// </summary>
        LevelSaveData LevelProgress { get; }

        /// <summary>
        /// Сохранить глобальный прогресс.
        /// </summary>
        void SaveGlobal();

        /// <summary>
        /// Сохранить прогресс уровня.
        /// </summary>
        void SaveLevel();

        /// <summary>
        /// Загрузить прогресс. 
        /// Если данных нет или они некорректны — вернёт ProgressGameData с начальными значениями.
        /// </summary>
        void LoadProgress();

        /// <summary>
        /// Сбросить прогресс до начальных значений и сохранить.
        /// </summary>
        void ResetProgress();
    }
}