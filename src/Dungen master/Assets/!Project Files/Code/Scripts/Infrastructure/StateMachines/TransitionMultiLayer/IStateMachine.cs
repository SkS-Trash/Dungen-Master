using System;
using Infrastructure.StateMachines.TransitionMultiLayer.ForState;

namespace Infrastructure.StateMachines.TransitionMultiLayer
{
    /// <summary>
    /// Интерфейс контроллера "машины состояний" с переходами.
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// Текущее активное состояние.
        /// </summary>
        IState CurrentState { get; }

        /// <summary>
        /// Событие изменения активного состояния.
        /// </summary>
        event Action<IState> OnStateChanged;

        /// <summary>
        /// Устанавливает начальное состояние, очищая стек.
        /// Вызываются OnInitialize и OnEnter у нового состояния.
        /// </summary>
        /// <param name="state">Начальное состояние.</param>
        void SetInitialState(IState state);

        /// <summary>
        /// Добавляет переход от конкретного состояния к целевому.
        /// Параметр replacing указывает, заменяет ли переход текущее состояние,
        /// а если целевое состояние равно null – выполняется удаление (pop).
        /// </summary>
        /// <param name="from">Исходное состояние.</param>
        /// <param name="to">Целевое состояние (или null для удаления).</param>
        /// <param name="replacing">Флаг замены состояния.</param>
        /// <param name="conditions">Набор условий для выполнения перехода.</param>
        void AddTransition(IState from, IState to, bool replacing = false, params Func<bool>[] conditions);

        /// <summary>
        /// Добавляет глобальный (непривязанный) переход, который проверяется независимо от текущего состояния.
        /// </summary>
        /// <param name="state">Целевое состояние, в которое осуществляется переход.</param>
        /// <param name="conditions">Условия для перехода.</param>
        void AddGlobalTransition(IState state, params Func<bool>[] conditions);

        /// <summary>
        /// Основной метод обновления, который должен вызываться каждый кадр.
        /// Проверяет переходы и выполняет Tick() активного состояния.
        /// </summary>
        void Tick();
    }
}