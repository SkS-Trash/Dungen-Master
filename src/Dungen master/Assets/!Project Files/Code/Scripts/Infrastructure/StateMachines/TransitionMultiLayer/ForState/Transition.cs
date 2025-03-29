using System;
using System.Linq;

namespace StateMachines.TransitionMultiLayer.ForState
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
        /// Флаг, указывающий, что переход должен заменить текущее состояние.
        /// </summary>
        public bool Replacing { get; }

        /// <summary>
        /// Возвращает true, если переход означает удаление состояния.
        /// </summary>
        public bool Removing => To == null;

        /// <summary>
        /// Массив условий, которые должны быть выполнены для осуществления перехода.
        /// </summary>
        public Func<bool>[] Conditions { get; }
        
        public Transition(IState to, bool replacing, params Func<bool>[] conditions)
        {
            To = to;
            Replacing = replacing;
            Conditions = conditions;
        }

        /// <summary>
        /// Проверяет, что все условия перехода выполнены.
        /// </summary>
        /// <returns>Возвращает true, если все условия выполнены.</returns>
        public bool CanTransition()
        {
            return Conditions.All(condition => condition());
        }
    }
}