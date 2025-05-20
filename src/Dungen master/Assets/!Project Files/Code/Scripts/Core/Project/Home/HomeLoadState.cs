using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

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

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            await _projectEngine.RunOneShot<LoadHomeSceneState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<SetupGameEventState>();
            await _projectEngine.RunOneShot<ConfiguredHomeState>();

            await _projectEngine.ChangeState<HomeState>();
        }

        public UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}