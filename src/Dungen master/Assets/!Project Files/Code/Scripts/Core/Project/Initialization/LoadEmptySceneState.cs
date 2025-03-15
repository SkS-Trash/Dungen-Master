using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Initialization
{
    public class LoadEmptySceneState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        public bool IsReusable => false;

        private readonly IProjectEngine _projectEngine;

        public LoadEmptySceneState(
            ISceneLoaderService sceneLoaderService
        )
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask Initialize()
        {
            var scenePath = ScenesPaths.EMPTY;

            await _sceneLoaderService.LoadSceneAsync(scenePath, saveInCache: false);
        }
    }
}