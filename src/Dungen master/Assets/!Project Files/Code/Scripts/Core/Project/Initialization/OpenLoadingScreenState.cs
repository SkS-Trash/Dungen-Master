using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Initialization
{
    public class OpenLoadingScreenState : IState
    {
        public bool IsReusable => false;

        private readonly IWindowService _windowService;

        public OpenLoadingScreenState(
            IWindowService windowService
        )
        {
            _windowService = windowService;
        }

        public async UniTask Initialize()
        {
            await InstantiateLoadingScreen();
        }

        private async UniTask InstantiateLoadingScreen()
        {
            await _windowService.Open(WindowID.GameLoading);
        }
    }
}