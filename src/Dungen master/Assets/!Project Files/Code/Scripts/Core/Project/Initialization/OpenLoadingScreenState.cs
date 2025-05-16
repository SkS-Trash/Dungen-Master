using Cysharp.Threading.Tasks;
using Services.Window;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Initialization
{
    public class OpenLoadingScreenState : IState, IEnterable
    {
        private readonly IWindowService _windowService;

        public OpenLoadingScreenState(
            IWindowService windowService
        )
        {
            _windowService = windowService;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await InstantiateLoadingScreen();
        }

        private async UniTask InstantiateLoadingScreen()
        {
            await _windowService.Open(WindowID.GameLoading);
        }
    }
}