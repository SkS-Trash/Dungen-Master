using Core.Project.Base;
using Core.Project.Home;
using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer.ForState;
using UI.MainMenu;

namespace Core.Project.MainMenu
{
    public class MainMenuState : IState, IEnterable, IExitable
    {
        private readonly IWindowService _windowService;
        private readonly IProjectEngine _projectEngine;

        public MainMenuState(
            IProjectEngine projectEngine,
            IWindowService windowService
        )
        {
            _windowService = windowService;
            _projectEngine = projectEngine;
        }

        #region Enter

        public async UniTask OnEnterAsync(Unit _)
        {
            await InstantiateMainMenu();

            SetupMainMenuCallbacks();

            ShowMainMenu();
            HideLoadingScreen();
        }

        private async UniTask InstantiateMainMenu()
        {
            var mainMenuUI = await _windowService.OpenAndGet<MainMenuUI>(WindowID.MainMenu);
            mainMenuUI.Hide();
        }

        private void SetupMainMenuCallbacks()
        {
            var mainMenuWindow = _windowService.Get<MainMenuUI>(WindowID.MainMenu);
            mainMenuWindow.OnStartGame += OnStartGame;
            mainMenuWindow.OnExit += OnExit;
        }

        private void OnStartGame()
        {
            _projectEngine.ChangeState<HomeLoadState>();
        }

        private void OnExit()
        {
            _projectEngine.ChangeState<ExitFromApplicationState>();
        }

        private void ShowMainMenu()
        {
            var mainMenuWindow = _windowService.Get<MainMenuUI>(WindowID.MainMenu);
            mainMenuWindow.Show();
        }

        private void HideLoadingScreen()
        {
            // TODO: Скрыть экран загрузки
        }

        #endregion

        #region Exit

        public UniTask OnExitAsync()
        {
            HideMainMenu();

            return UniTask.CompletedTask;
        }

        private void HideMainMenu()
        {
            var mainMenuWindow = _windowService.Get<MainMenuUI>(WindowID.MainMenu);
            mainMenuWindow.Hide();
        }

        #endregion
    }
}