namespace StateMachines.Transition
{
    /// <summary>
    /// Интерфейс для описания состояний.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Вход в состояние.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Логика состояния, вызываемая каждый кадр.
        /// </summary>
        void OnExecute();

        /// <summary>
        /// Выход из состояния.
        /// </summary>
        void OnExit();
    }
}