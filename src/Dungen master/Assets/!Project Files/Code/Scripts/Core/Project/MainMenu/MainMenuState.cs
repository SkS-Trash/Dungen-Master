using Core.Project.Home;
using Cysharp.Threading.Tasks;
using Services.Progress;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;
using Subscribers;
using Subscribers.EventBusSystem;
using UI.MainMenu;

namespace Core.Project.MainMenu
{
    public class MainMenuState : IState, IEnterable, IExitable,
        ILaunchNewGame, ILaunchContinueGame, IQuitApplication
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

            await InstantiateMainMenu();

            HideLoadingScreen();

            SubscribeToEvents();
        }

        private async UniTask InstantiateMainMenu()
        {
            var mainMenuUI = await _windows.OpenAndGet<MainMenuUI>(WindowID.MainMenu);
            mainMenuUI.ContinueGameButtonInteractable(!_progress.GlobalProgress.isFirstLaunch);
        }

        public void LaunchNewGame()
        {
            _projectEngine.ChangeState<HomeLoadState>();
            // _projectEngine.ChangeState<TestState>();
        }

        public void LaunchContinueGame()
        {
            _projectEngine.ChangeState<HomeLoadState>();
            // _projectEngine.ChangeState<TestState>();
        }

        public void QuitApplication()
        {
            _projectEngine.ChangeState<ExitFromApplicationState>();
        }

        private void HideLoadingScreen()
        {
            // TODO: Скрыть экран загрузки
        }

        private void SubscribeToEvents()
        {
            EventBus.Subscribe(this);
        }

        #endregion

        #region Exit

        public UniTask OnExitAsync()
        {
            UnsubscribeFromEvents();

            HideMainMenu();

            return UniTask.CompletedTask;
        }

        private void HideMainMenu()
        {
            _windows.Close(WindowID.MainMenu);
        }

        public void UnsubscribeFromEvents()
        {
            EventBus.Unsubscribe(this);
        }

        #endregion
    }
}