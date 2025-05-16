using Core.Project.Home;
using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Observers.Input;
using Services.CursorControl;
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
        private readonly ICursorControlService _cursorControl;

        private CursorSnapshot _cursorSnapshot;

        public GamePauseState(
            IProjectEngine projectEngine,
            IWindowService window,
            IInputActionReader inputActionReader,
            ICursorControlService cursorControl
        )
        {
            _projectEngine = projectEngine;
            _window = window;
            _inputActionReader = inputActionReader;
            _cursorControl = cursorControl;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            _cursorSnapshot = _cursorControl.Capture();

            await _window.Open(WindowID.GamePauseMenu);

            _inputActionReader.OnCancelChanged += ExitFromPause;

            EventBus.Subscribe(this);
            EventBus.RaiseEvent<IPauseGameSubscriber>(x => x.OnPauseGame(true));
        }

        private void OnExit()
        {
            _window.Close(WindowID.GamePauseMenu);

            _cursorControl.Restore(_cursorSnapshot);

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