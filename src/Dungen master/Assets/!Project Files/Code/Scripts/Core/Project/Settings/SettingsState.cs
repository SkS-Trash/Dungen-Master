using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using Services.Window;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Settings
{
    public class SettingsState : IState, IEnterable, IExitable,
        ISettingsCloseEvent
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IWindowService _window;

        public SettingsState(
            IProjectEngine projectEngine,
            IWindowService window
        )
        {
            _projectEngine = projectEngine;
            _window = window;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            await _window.Open(WindowID.Settings);

            EventBus.Subscribe(this);
        }

        public UniTask OnExitAsync()
        {
            EventBus.Unsubscribe(this);

            _window.Close(WindowID.Settings);

            return UniTask.CompletedTask;
        }

        public void OnCloseSettings()
        {
            _projectEngine.ChangeState<MainMenuState>();
        }
    }
}