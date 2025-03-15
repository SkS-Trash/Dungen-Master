using Core.Project.Initialization;
using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Base
{
    public class BootstrapState : IState
    {
        public bool IsReusable => false;

        private readonly IProjectEngine _stateMachine;

        public BootstrapState(
            IProjectEngine stateMachine
        )
        {
            _stateMachine = stateMachine;
        }

        public UniTask Initialize()
        {
            _stateMachine.ChangeState<InitializationState>();
            
            return UniTask.CompletedTask;
        }
    }
}