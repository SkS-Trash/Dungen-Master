#region

using System;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;

#endregion

// ReSharper disable SuspiciousTypeConversion.Global

namespace Infrastructure.StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Интерфейс контроллера "машины состояний"
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// Событие изменения состояния.
        /// </summary>
        event Action<IState> StateChanged;

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        Type CurrentStateType { get; }

        // ------------------- 

        /// <summary>
        /// Переход в новое состояние без аргументов.
        /// </summary>
        UniTask ChangeState<TState>() where TState : IState;

        /// <summary>
        /// Переход в новое состояние с аргументами.
        /// </summary>
        UniTask ChangeState<TState, TArg>(TArg arg) where TState : IState;

        // ------------------- 

        /// <summary>
        /// Инициализирует состояние, не запоминая его в кеше, и возвращает UniTask для отслеживания завершения.
        /// </summary>
        UniTask RunOneShot<TState>() where TState : IStateOneShot;

        /// <summary>
        /// Инициализирует состояние, не запоминая его в кеше, и возвращает UniTask для отслеживания завершения.
        /// </summary>
        UniTask RunOneShot<TState, TArg>(TArg arg) where TState : IStateOneShot<TArg>;

        // ------------------- 

        /// <summary>
        /// Добавить состояние в стек.
        /// </summary>
        UniTask PushState<TState>() where TState : IState;

        /// <summary>
        /// Добавить состояние в стек с аргументами.
        /// </summary>
        UniTask PushState<TState, TArg>(TArg arg) where TState : IState;

        /// <summary>
        /// Убрать состояние из стека.
        /// </summary>
        UniTask PopState();
    }
}