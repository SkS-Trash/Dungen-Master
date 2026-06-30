using Cysharp.Threading.Tasks;
using Services.SceneLoader;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Home
{
    public class LoadHomeSceneState : IStateOneShot
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public LoadHomeSceneState(
            ISceneLoaderService sceneLoaderService
        )
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            await _sceneLoaderService.LoadSceneAsync(ScenesPaths.HOME);
        }
    }
}