using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Initialization
{
    public class InitializationState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;

        public InitializationState(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            // await _projectEngine.RunWhileWaitingForCompletion<OpenLoadingScreenState>();
            await _projectEngine.RunWhileWaitingForCompletion<LoadEmptySceneState>();
            await _projectEngine.RunWhileWaitingForCompletion<LoadingBasicResourcesState>();
            await _projectEngine.RunWhileWaitingForCompletion<LoadProgressState>();

            _projectEngine.ChangeState<MainMenuState>();
        }
    }
}