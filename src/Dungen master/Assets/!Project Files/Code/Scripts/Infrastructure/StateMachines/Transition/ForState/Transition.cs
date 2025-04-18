using System;
using System.Linq;

namespace StateMachines.Transition
{
    /// <summary>
    /// Внутренний класс, описывающий переход между состояниями.
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Целевое состояние для перехода (если равно null, означает удаление текущего состояния).
        /// </summary>
        public IState To { get; }

        /// <summary>
        /// Массив условий, которые должны быть выполнены для осуществления перехода.
        /// </summary>
        public Func<bool>[] Conditions { get; }

        public Transition(IState to, params Func<bool>[] conditions)
        {
            To = to;
            Conditions = conditions;
        }

        /// <summary>
        /// Проверяет, что все условия перехода выполнены.
        /// </summary>
        /// <returns>Возвращает true, если все условия выполнены.</returns>
        public bool CanTransition() =>
            Conditions.All(condition => condition.Invoke());
    }
}