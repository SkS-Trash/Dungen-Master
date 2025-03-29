namespace StateMachines.DirectControlMultiLayer.ForState
{
    /// <summary>
    /// Фабрика для создания состояний.
    /// </summary>
    public interface IStatesFactory
    {
        /// <summary>
        /// Создать новое состояние проекта.
        /// </summary>
        /// <typeparam name="TState">Тип состояния.</typeparam>
        /// <returns>Созданное состояние.</returns>
        TState CreateState<TState>() where TState : IState;
    }
}