using Progress;
using Providers.Data;

namespace Services.Progress
{
    /// <summary>
    /// Сервис для работы с прогрессом, хранящимся в Scriptable Object.
    /// </summary>
    public class ProgressInScriptableObjectsService : IProgressService
    {
        public GlobalSaveData GlobalProgress => _progressGameDataHolder.GlobalSaveData;
        public LevelSaveData LevelProgress => _progressGameDataHolder.LevelSaveData;

        private static LevelProgressSaveCollectorsProvider LevelProgressSaveCollectors =>
            LevelProgressSaveCollectorsProvider.Instance;

        private readonly IStaticDataProvider _staticDataProvider;
        private ProgressGameDataHolder _progressGameDataHolder;

        public ProgressInScriptableObjectsService(
            IStaticDataProvider staticDataProvider
        )
        {
            _staticDataProvider = staticDataProvider;

            LoadProgress();
        }

        /// <inheritdoc/>
        public void SaveGlobal()
        {
            GlobalProgress.isFirstLaunch = false;
            GlobalProgress.version = GlobalSaveData.VERSION;
        }

        public void SaveLevel()
        {
            LevelProgressSaveCollectors.Collect(LevelProgress);
        }

        /// <inheritdoc/>
        public void LoadProgress()
        {
            _progressGameDataHolder = _staticDataProvider.GetProgressGameDataHolder();

            // Ничего не делаем, так как данные хранятся в Scriptable Object.
        }

        /// <inheritdoc/>
        public void ResetProgress()
        {
            _progressGameDataHolder.ResetProgress();
        }
    }
}