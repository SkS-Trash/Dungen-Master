using Core.Project.Initialization;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.ProjectManager;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;

namespace Core.Project.Base
{
    public class BootstrapState : IState, IEnterable
    {
        private readonly IProjectEngine _stateMachine;

        public BootstrapState(
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