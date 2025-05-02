using Core.Project.Home;
using Core.Project.Settings;
using Cysharp.Threading.Tasks;
using Services.Progress;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.MainMenu
{
    public class MainMenuState : IState, IEnterable, IExitable,
        ILaunchNewGame, ILaunchContinueGame, IQuitApplication, IOpenSettingsSubscriber
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IWindowService _windows;
        private readonly IProgressService _progress;

        public MainMenuState(
            IProjectEngine projectEngine,
            IWindowService windows,
            IProgressService progress
        )
        {
            _windows = windows;
            _progress = progress;
            _projectEngine = projectEngine;
        }

        #region Enter

        public async UniTask OnEnterAsync(Unit _)
        {
            await _projectEngine.RunOneShot<LoadHomeSceneState>();

            await _windows.Open(WindowID.MainMenu);

            EventBus.RaiseEvent<IGlobalProgressLoadSubscriber>(s => s.OnProgressLoaded(_progress.GlobalProgress));

            // HideLoadingScreen();

            EventBus.Subscribe(this);
        }

        public void LaunchNewGame() =>
            _projectEngine.ChangeState<LaunchNewGameState>();

        public void LaunchContinueGame() =>
            _projectEngine.ChangeState<LaunchContinueGameState>();

        public void QuitApplication() =>
            _projectEngine.ChangeState<ExitFromApplicationState>();

        public void OpenSettings() =>
            _projectEngine.ChangeState<SettingsState>();
        
        #endregion

        #region Exit

        public UniTask OnExitAsync()
        {
            EventBus.Unsubscribe(this);

            _windows.Close(WindowID.MainMenu);

            return UniTask.CompletedTask;
        }

        #endregion
    }
}