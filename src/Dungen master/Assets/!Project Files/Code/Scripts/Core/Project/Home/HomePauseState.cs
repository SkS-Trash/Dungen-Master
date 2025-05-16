using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Observers.Input;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Home
{
    public class HomePauseState : IStateOneShot,
        IExitInMainMenuEvent,
        IExitFromPauseScreenEvent
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IWindowService _window;
        private readonly IInputActionReader _inputActionReader;

        private HomePauseState(
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
            await _window.Open(WindowID.HomePauseMenu);
            _inputActionReader.OnCancelChanged += ExitFromPause;
            EventBus.Subscribe(this);

            EventBus.RaiseEvent<IPauseGameEvent>(x => x.OnPauseGame(true));
        }

        private void OnExit()
        {
            _window.Close(WindowID.HomePauseMenu);
            _inputActionReader.OnCancelChanged -= ExitFromPause;
            EventBus.Unsubscribe(this);

            EventBus.RaiseEvent<IPauseGameEvent>(x => x.OnPauseGame(false));
        }

        public void OnExitInMainMenu()
        {
            OnExit();

            _projectEngine.ChangeState<MainMenuState>();
        }

        public void OnExitFromPauseScreen() => OnExit();

        private void ExitFromPause() => OnExit();
    }
}