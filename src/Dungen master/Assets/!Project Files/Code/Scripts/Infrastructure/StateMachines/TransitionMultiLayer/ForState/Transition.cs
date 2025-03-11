using System;

namespace StateMachines.TransitionMultiLayer
{
    /// <summary>
    /// Внутренний класс, описывающий переход между состояниями.
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Массив условий, которые должны быть выполнены для осуществления перехода.
        /// </summary>
        public Func<bool>[] Conditions { get; }

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

        public Transition(IState to, bool replacing, params Func<bool>[] conditions)
        {
            To = to;
            Replacing = replacing;
            Conditions = conditions;
        }
    }
}