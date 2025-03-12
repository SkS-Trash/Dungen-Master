using UnityEngine;

namespace Services
{
    /// <summary>
    /// Сервис для работы с прогрессом игры, сохраняемым в локальном хранилище.
    /// </summary>
    public class ProgressInLocalStorageService : IProgressService
    {
        private const string DATA_KEY = "ProgressGameData";

        private readonly ISaveLoadDataService _saveLoadDataService;

        /// <inheritdoc/>
        public ProgressGameData CurrentProgress { get; private set; }

        public ProgressInLocalStorageService(ISaveLoadDataService saveLoadDataService)
        {
            _saveLoadDataService = saveLoadDataService;

            LoadProgress();
        }

        /// <inheritdoc/>
        public void SaveProgress()
        {
            if (CurrentProgress == null)
            {
                Debug.LogWarning("Нет данных о прогрессе для сохранения.");
                return;
            }

            _saveLoadDataService.Save(CurrentProgress, DATA_KEY);
        }

        /// <inheritdoc/>
        public void LoadProgress()
        {
            var loadedData = _saveLoadDataService.Load<ProgressGameData>(DATA_KEY);

            if (loadedData == null)
            {
                Debug.LogWarning("Сохранённые данные не найдены. Создаём новый прогресс.");
                CurrentProgress = new ProgressGameData();
            }
            else
            {
                CurrentProgress = loadedData;
            }
        }

        /// <inheritdoc/>
        public void ResetProgress()
        {
            CurrentProgress = new ProgressGameData();
            SaveProgress();
        }
    }
}