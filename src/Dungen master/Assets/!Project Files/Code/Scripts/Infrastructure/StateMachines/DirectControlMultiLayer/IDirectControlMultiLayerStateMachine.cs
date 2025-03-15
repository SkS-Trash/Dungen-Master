#region

using System;
using Cysharp.Threading.Tasks;

#endregion

// ReSharper disable SuspiciousTypeConversion.Global

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Интерфейс контроллера "машины состояний"
    /// </summary>
    public interface IDirectControlMultiLayerStateMachine
    {
        /// <summary>
        /// Событие изменения состояния.
        /// </summary>
        event Action<IState> StateChanged;

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        Type CurrentStateType { get; }

        /// <summary>
        /// Переход в новое состояние без аргументов.
        /// </summary>
        void ChangeState<TState>() where TState : IState;

        /// <summary>
        /// Переход в новое состояние с аргументами.
        /// </summary>
        void ChangeState<TState, TArg>(TArg arg) where TState : IState;

        /// <summary>
        /// Добавить состояние в стек.
        /// </summary>
        void PushState<TState>() where TState : IState;

        /// <summary>
        /// Добавить состояние в стек с аргументами.
        /// </summary>
        void PushState<TState, TArg>(TArg arg) where TState : IState;

        /// <summary>
        /// Убрать состояние из стека.
        /// </summary>
        void PopState();
        
        /// <summary>
        /// Инициализирует состояние, не запоминая его в кеше, и возвращает UniTask для отслеживания завершения.
        /// </summary>
        UniTask InitializeStateWithoutCaching<TState>() where TState : IState;
    }
}