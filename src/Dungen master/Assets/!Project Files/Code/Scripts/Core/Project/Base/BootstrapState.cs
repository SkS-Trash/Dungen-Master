using Cysharp.Threading.Tasks;
using Services;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project
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
            // _stateMachine.ChangeState<OpenLoadingScreenState>();

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}