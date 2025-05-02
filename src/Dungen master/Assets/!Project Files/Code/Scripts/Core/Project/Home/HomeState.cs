using Cysharp.Threading.Tasks;
using Observers.Input;
using Services.Window;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Home
{
    public class HomeState : IState, IEnterable, IExitable
    {
        private readonly IInputActionReader _inputReader;
        private readonly IWindowService _windowService;

        public HomeState(
            IInputActionReader inputReader,
            IWindowService windowService
        )
        {
            _inputReader = inputReader;
            _windowService = windowService;
        }

        public UniTask OnEnterAsync(Unit _)
        {
            _inputReader.OnCancelChanged += OnCancelChanged;

            return UniTask.CompletedTask;
        }

        public UniTask OnExitAsync()
        {
            _inputReader.OnCancelChanged -= OnCancelChanged;

            return UniTask.CompletedTask;
        }

        private void OnCancelChanged()
        {
            _windowService.Open(WindowID.HomePauseMenu);
        }
    }
}