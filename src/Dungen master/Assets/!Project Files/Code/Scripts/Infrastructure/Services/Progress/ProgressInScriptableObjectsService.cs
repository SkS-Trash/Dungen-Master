using Progress;
using Providers.Data;

namespace Services.Progress
{
    /// <summary>
    /// Сервис для работы с прогрессом, хранящимся в Scriptable Object.
    /// </summary>
    public class ProgressInScriptableObjectsService : IProgressService
    {
        /// <inheritdoc/>
        public ProgressGameData CurrentProgress => _progressGameDataHolder.ProgressGameData;

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
        public void SaveProgress()
        {
            // Ничего не делаем, так как данные хранятся в Scriptable Object.
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