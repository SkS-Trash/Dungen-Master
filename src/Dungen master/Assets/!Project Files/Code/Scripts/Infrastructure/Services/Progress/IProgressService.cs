namespace Services
{
    /// <summary>
    /// Сервис для работы с прогрессом игры.
    /// </summary>
    public interface IProgressService
    {
        /// <summary>
        /// Текущий прогресс.
        /// </summary>
        ProgressGameData CurrentProgress { get; }

        /// <summary>
        /// Сохранить прогресс.
        /// </summary>
        void SaveProgress();

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