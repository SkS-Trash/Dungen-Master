using Core.Project.Home;
using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Observers.Input;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Dungeon
{
    public class GamePauseState : IStateOneShot,
        IExitInMainMenuSubscriber,
        IExitFromPauseScreenSubscriber,
        IExitInHomeSubscriber
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IWindowService _window;
        private readonly IInputActionReader _inputActionReader;

        private GamePauseState(
            IProjectEngine projectEngine,
            IWindowService window,
            IInputActionReader inputActionReader
        )
        {
            _projectEngine = projectEngine;
            _window = window;
            _inputActionReader = inputActionReader;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await _window.Open(WindowID.GamePauseMenu);

            _inputActionReader.OnCancelChanged += ExitFromPause;
            EventBus.Subscribe(this);

            EventBus.RaiseEvent<IPauseGameSubscriber>(x => x.OnPauseGame(true));
        }

        private void OnExit()
        {
            _window.Close(WindowID.GamePauseMenu);

            _inputActionReader.OnCancelChanged -= ExitFromPause;
            EventBus.Unsubscribe(this);

            EventBus.RaiseEvent<IPauseGameSubscriber>(x => x.OnPauseGame(false));
        }

        public void OnExitInMainMenu()
        {
            OnExit();

            _projectEngine.ChangeState<MainMenuState>();
        }

        public void OnExitInHome()
        {
            OnExit();

            _projectEngine.ChangeState<HomeState>();
        }

        public void OnExitFromPauseHomeScreen() => OnExit();

        private void ExitFromPause() => OnExit();
    }
}