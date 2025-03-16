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
            // await _projectEngine.InitializeStateWithoutCaching<Ехать OpenLoadingScreenState>();
            await _projectEngine.RunWhileWaitingForCompletion<LoadingBasicResourcesState>();
            await _projectEngine.RunWhileWaitingForCompletion<LoadProgressState>();
            // await _projectEngine.InitializeStateWithoutCaching<LoadEmptySceneState>();

            await _projectEngine.RunWhileWaitingForCompletion<WindowServiceInitializeState>();
            _projectEngine.ChangeState<MainMenuState>();
        }

        // Костыль для инициализации сервиса окон
        private class WindowServiceInitializeState : IState
        {
            public WindowServiceInitializeState(IWindowService windowService)
            {
            }
        }
    }
}