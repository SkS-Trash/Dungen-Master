using Reflex.Core;
using Reflex.Injectors;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Реализация фабрики состояний.
    /// </summary>
    public class StatesFactory : IStatesFactory
    {
        private readonly Container _container;

        public StatesFactory(Container container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public TState CreateState<TState>() where TState : IState
            => (TState)ConstructorInjector.Construct(typeof(TState), _container);
    }
}