using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer;

namespace Services.ProjectManager
{
    public class ProjectEngineAdapter : IProjectEngine
    {
        private readonly IStateMachine _impl;

        public ProjectEngineAdapter(IStateMachine impl)
        {
            _impl = impl;
        }

        public event System.Action<IState> StateChanged
        {
            add => _impl.StateChanged += value;
            remove => _impl.StateChanged -= value;
        }

        public System.Type CurrentStateType => _impl.CurrentStateType;

        public UniTask ChangeState<TState>() where TState : IState =>
            _impl.ChangeState<TState>();

        public UniTask ChangeState<TState, TArg>(TArg arg) where TState : IState =>
            _impl.ChangeState<TState, TArg>(arg);

        public UniTask RunOneShot<TState>() where TState : IStateOneShot
            => _impl.RunOneShot<TState>();

        public UniTask RunOneShot<TState, TArg>(TArg arg) where TState : IStateOneShot<TArg>
            => _impl.RunOneShot<TState, TArg>(arg);

        public UniTask PushState<TState>() where TState : IState =>
            _impl.PushState<TState>();

        public UniTask PushState<TState, TArg>(TArg arg) where TState : IState =>
            _impl.PushState<TState, TArg>(arg);

        public UniTask PopState() =>
            _impl.PopState();
    }
}