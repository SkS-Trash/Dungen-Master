using Core.Project.Initialization;
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

        public UniTask OnEnterAsync(Unit _)
        {
            _stateMachine.ChangeState<InitializationState>();

            return UniTask.CompletedTask;
        }
    }
}