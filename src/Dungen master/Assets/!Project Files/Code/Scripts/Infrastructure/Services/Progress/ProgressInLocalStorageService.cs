using Progress;
using Services.SaveLoadData;
using UnityEngine;

namespace Services.Progress
{
    /// <summary>
    /// Сервис для работы с прогрессом игры, сохраняемым в локальном хранилище.
    /// </summary>
    public class ProgressInLocalStorageService : IProgressService
    {
        public GlobalSaveData GlobalProgress { get; private set; }
        public LevelSaveData LevelProgress { get; private set; }

        private const string GLOBAL_SAVE_DATA_KEY = "GlobalSaveData";
        private const string LEVEL_SAVE_DATA_KEY = "LevelSaveData";

        private static LevelProgressSaveCollectorsProvider LevelProgressSaveCollectors =>
            LevelProgressSaveCollectorsProvider.Instance;

        private readonly ISaveLoadDataService _saveLoad;

        public ProgressInLocalStorageService(ISaveLoadDataService saveLoad)
        {
            _saveLoad = saveLoad;

            LoadProgress();
        }

        /// <inheritdoc/>
        public void SaveGlobal()
        {
            GlobalProgress.isFirstLaunch = false;
            GlobalProgress.version = GlobalSaveData.VERSION;
            _saveLoad.Save(GlobalProgress, GLOBAL_SAVE_DATA_KEY);
        }

        /// <inheritdoc/>
        public void SaveLevel()
        {
            LevelProgressSaveCollectors.Collect(LevelProgress);
            _saveLoad.Save(LevelProgress, LEVEL_SAVE_DATA_KEY);
        }

        /// <inheritdoc/>
        public void LoadProgress()
        {
            GlobalProgress = _saveLoad.Load<GlobalSaveData>(GLOBAL_SAVE_DATA_KEY);

            if (GlobalProgress == null)
            {
                GlobalProgress = new GlobalSaveData
                {
                    isFirstLaunch = true,
                    version = GlobalSaveData.VERSION
                };

                LevelProgress = new LevelSaveData();

                return;
            }

            if (GlobalProgress.version != GlobalSaveData.VERSION)
            {
                Debug.LogWarning(
                    $"Версия сохранённых данных ({GlobalProgress.version}) не совпадает с текущей ({GlobalSaveData.VERSION}). " +
                    $"Сбросим прогресс до начальных значений.");

                ResetProgress();

                return;
            }

            LevelProgress = _saveLoad.Load<LevelSaveData>(LEVEL_SAVE_DATA_KEY) ?? new LevelSaveData();
        }

        /// <inheritdoc/>
        public void ResetProgress()
        {
            GlobalProgress = new GlobalSaveData
            {
                isFirstLaunch = true,
                version = GlobalSaveData.VERSION
            };

            LevelProgress = new LevelSaveData();

            _saveLoad.Save(GlobalProgress, GLOBAL_SAVE_DATA_KEY);
            _saveLoad.Save(LevelProgress, LEVEL_SAVE_DATA_KEY);
        }
    }
}