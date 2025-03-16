using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer;

namespace Services
{
    public class ProjectEngineAdapter : IProjectEngine
    {
        private readonly IDirectControlMultiLayerStateMachine _impl;

        public ProjectEngineAdapter(IDirectControlMultiLayerStateMachine impl)
        {
            _impl = impl;
        }

        public event System.Action<IState> StateChanged
        {
            add => _impl.StateChanged += value;
            remove => _impl.StateChanged -= value;
        }

        public System.Type CurrentStateType => _impl.CurrentStateType;

        public void ChangeState<TState>() where TState : IState =>
            _impl.ChangeState<TState>();

        public void ChangeState<TState, TArg>(TArg arg) where TState : IState =>
            _impl.ChangeState<TState, TArg>(arg);

        public void PushState<TState>() where TState : IState =>
            _impl.PushState<TState>();

        public void PushState<TState, TArg>(TArg arg) where TState : IState =>
            _impl.PushState<TState, TArg>(arg);

        public void PopState() =>
            _impl.PopState();

        public UniTask RunWhileWaitingForCompletion<TState>() where TState : IState
            => _impl.RunWhileWaitingForCompletion<TState>();
    }
}