using Cysharp.Threading.Tasks;
using Infrastructure.Services.ProjectManager;
using Infrastructure.Services.SceneLoader;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Initialization
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