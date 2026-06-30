using Core.Project.Home;
using Cysharp.Threading.Tasks;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project
{
    public class LaunchNewGameState : IState, IEnterable
    {
        private readonly IProjectEngine _stateMachine;

        public LaunchNewGameState(
            IProjectEngine stateMachine
        )
        {
            _stateMachine = stateMachine;
        }

        public UniTask OnEnterAsync(UnitEmpty _)
        {
            _stateMachine.ChangeState<HomeLoadState>();

            return UniTask.CompletedTask;
        }
    }
}