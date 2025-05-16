using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using Services.SceneLoader;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project
{
    public class LoadEmptySceneState : IStateOneShot
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