using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachines.TransitionMultiLayer
{
    /// <summary>
    /// Контроллер "машины состояний" с переходами.
    /// </summary>
    public sealed class MultiLayerTransitionStateMachine : IMultiLayerTransitionStateMachine
    {
        /// <inheritdoc/>
        public IState CurrentState => _stateStack.Count > 0
            ? _stateStack.Peek()
            : null;

        /// <inheritdoc/>
        public event Action<IState> OnStateChanged;

        private readonly Stack<IState> _stateStack = new();
        private readonly List<Transition> _unspecifiedTransitions = new();
        private readonly Dictionary<Type, List<Transition>> _stateTransitions = new();

        /// <inheritdoc/>
        public void SetInitialState(IState state)
        {
            while (_stateStack.Count > 0)
            {
                var s = _stateStack.Pop();
                s.OnExit();
            }

            _stateStack.Push(state);

            EnterNewState(state);
        }

        /// <inheritdoc/>
        public void AddTransition(IState from, IState to, bool replacing = false, params Func<bool>[] conditions)
        {
            if (!_stateTransitions.TryGetValue(from.GetType(), out var transitions))
            {
                transitions = new List<Transition>();

                _stateTransitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, replacing, conditions));
        }

        /// <inheritdoc/>
        public void AddUnspecifiedTransition(IState state, params Func<bool>[] conditions)
        {
            _unspecifiedTransitions.Add(new Transition(state, false, conditions));
        }

        /// <inheritdoc/>
        public void Tick()
        {
            var transition = GetValidTransition();
            if (transition != null)
            {
                Debug.Log($"Переход от {CurrentState.GetType().Name} к {transition.To.GetType().Name}");

                if (transition.Removing)
                {
                    RemoveState();
                }
                else if (transition.Replacing)
                {
                    ReplaceState(transition.To);
                }
                else
                {
                    PushState(transition.To);
                }
            }

            CurrentState?.OnExecute();
        }

        private void ReplaceState(IState newState)
        {
            if (newState == CurrentState) return;

            ExitFromState();

            _stateStack.Pop();
            _stateStack.Push(newState);

            EnterNewState(newState);
        }

        private void PushState(IState newState)
        {
            if (newState == CurrentState) return;

            ExitFromState();

            _stateStack.Push(newState);

            EnterNewState(newState);
        }

        private void RemoveState()
        {
            if (_stateStack.Count <= 1) return;

            ExitFromState();

            _stateStack.Pop();

            EnterNewState(CurrentState);
        }

        private void EnterNewState(IState newState)
        {
            newState?.OnEnter();

            OnStateChanged?.Invoke(newState);
        }

        private void ExitFromState()
        {
            CurrentState?.OnExit();
        }


        private Transition GetValidTransition()
        {
            foreach (var transition in _unspecifiedTransitions.Where(AllConditionsMet))
            {
                return transition;
            }

            if (CurrentState == null)
            {
                return null;
            }

            if (_stateTransitions.TryGetValue(CurrentState.GetType(), out var transitions))
            {
                return transitions.FirstOrDefault(AllConditionsMet);
            }

            return null;
        }

        private static bool AllConditionsMet(Transition transition)
        {
            return transition.Conditions.All(condition => condition());
        }
    }
}