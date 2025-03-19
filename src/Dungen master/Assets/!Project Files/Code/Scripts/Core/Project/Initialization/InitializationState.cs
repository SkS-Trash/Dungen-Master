using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.ProjectManager;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;
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
            await _projectEngine.RunOneShot<LoadEmptySceneState>();
            await _projectEngine.RunOneShot<LoadingBasicResourcesState>();
            await _projectEngine.RunOneShot<LoadProgressState>();

            _projectEngine.ChangeState<MainMenuState>();
        }
    }
}