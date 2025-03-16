using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Initialization
{
    public class LoadEmptySceneState : IState, IEnterable
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IProjectEngine _projectEngine;

        public LoadEmptySceneState(
            ISceneLoaderService sceneLoaderService
        )
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            var scenePath = ScenesPaths.EMPTY;

            await _sceneLoaderService.LoadSceneAsync(scenePath, saveInCache: false);
        }
    }
}