using VContainer;

namespace Infrastructure.StateMachines.DirectControlMultiLayer.ForState
{
    /// <summary>
    /// Реализация фабрики состояний.
    /// </summary>
    public class StatesFactory : IStatesFactory
    {
        private readonly IObjectResolver _container;

        public StatesFactory(IObjectResolver container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public TState CreateState<TState>() where TState : IState
        {
            return _container.Resolve<TState>();
        }
    }
}