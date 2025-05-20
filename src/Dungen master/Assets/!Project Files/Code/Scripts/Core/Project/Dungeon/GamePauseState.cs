using Core.Project.Home;
using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Observers.Input;
using Services.CursorControl;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class GamePauseState : IStateOneShot,
        IExitInMainMenuEvent,
        IExitFromPauseScreenEvent,
        IExitInHomeEvent
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

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            CursorSetup();

            await _window.Open(WindowID.GamePauseMenu);

            _inputActionReader.OnCancelChanged += ExitFromPause;

            EventBus.Subscribe(this);
            EventBus.RaiseEvent<IPauseGameEvent>(x => x.OnPauseGame(true));
        }

        private void CursorSetup()
        {
            _cursorSnapshot = _cursorControl.Capture();
            _cursorControl.SetLock(CursorLockMode.None);
            _cursorControl.SetVisible(true);
        }

        private void OnExit()
        {
            _window.Close(WindowID.GamePauseMenu);

            _cursorControl.Restore(_cursorSnapshot);

            _inputActionReader.OnCancelChanged -= ExitFromPause;
            EventBus.Unsubscribe(this);

            EventBus.RaiseEvent<IPauseGameEvent>(x => x.OnPauseGame(false));
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

        public void OnExitFromPauseScreen() => OnExit();

        private void ExitFromPause() => OnExit();
    }
}