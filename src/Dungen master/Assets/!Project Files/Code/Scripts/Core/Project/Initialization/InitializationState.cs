using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Initialization
{
    public class InitializationState : IState
    {
        public bool IsReusable => false;

        private readonly IProjectEngine _projectEngine;

        public InitializationState(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;
        }

        public async UniTask Initialize()
        {
            // await _projectEngine.InitializeStateWithoutCaching<OpenLoadingScreenState>();
            await _projectEngine.InitializeStateWithoutCaching<LoadingBasicResourcesState>();
            await _projectEngine.InitializeStateWithoutCaching<LoadProgressState>();
            await _projectEngine.InitializeStateWithoutCaching<LoadEmptySceneState>();

            // _projectEngine.ChangeState<MainMenuState>();

            Debug.Log("InitializationState initialized");
        }
    }
}