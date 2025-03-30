using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Home
{
    public class HomeLoadState : IState, IEnterable, IExitable
    {
        private readonly IProjectEngine _projectEngine;

        public HomeLoadState(
            IProjectEngine projectEngine
        )
        {
            _projectEngine = projectEngine;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await _projectEngine.RunOneShot<LoadHomeSceneState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            
            _projectEngine.ChangeState<HomeState>();
        }

        public UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}