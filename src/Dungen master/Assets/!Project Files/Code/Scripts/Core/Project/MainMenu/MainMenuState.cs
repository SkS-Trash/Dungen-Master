using Core.Project.Home;
using Core.Project.Settings;
using Cysharp.Threading.Tasks;
using Services.CursorControl;
using Services.Progress;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.MainMenu
{
    public class MainMenuState : IState, IEnterable, IExitable,
        ILaunchNewGameEvent, ILaunchContinueGameEvent, IQuitApplicationEvent, IOpenSettingsEvent
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IWindowService _windows;
        private readonly IProgressService _progress;
        private readonly ICursorControlService _cursorControl;

        public MainMenuState(
            IProjectEngine projectEngine,
            IWindowService windows,
            IProgressService progress,
            ICursorControlService cursorControl
        )
        {
            _windows = windows;
            _progress = progress;
            _cursorControl = cursorControl;
            _projectEngine = projectEngine;
        }

        #region Enter

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            _cursorControl.SetLock(CursorLockMode.None);
            _cursorControl.SetVisible(true);

            await _projectEngine.RunOneShot<LoadHomeSceneState>();

            await _windows.Open(WindowID.MainMenu);

            EventBus.RaiseEvent<IGlobalProgressLoadEvent>(s => s.OnProgressLoaded(_progress.GlobalProgress));

            // HideLoadingScreen();

            EventBus.Subscribe(this);
        }

        public void OnLaunchNewGame() =>
            _projectEngine.ChangeState<LaunchNewGameState>();

        public void OnLaunchContinueGame() =>
            _projectEngine.ChangeState<LaunchContinueGameState>();

        public void OnQuitApplication() =>
            _projectEngine.ChangeState<ExitFromApplicationState>();

        public void OnOpenSettings() =>
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