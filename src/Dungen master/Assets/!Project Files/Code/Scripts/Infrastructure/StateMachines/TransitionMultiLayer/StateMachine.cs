using System;
using System.Collections.Generic;
using StateMachines.TransitionMultiLayer.ForState;
using UnityEngine;

namespace StateMachines.TransitionMultiLayer
{
    public sealed class StateMachine : IStateMachine
    {
        public IState CurrentState => _stateStack.Count > 0
            ? _stateStack[^1]
            : null;

        public event Action<IState> OnStateChanged;

        private readonly List<IState> _stateStack = new();
        private readonly Dictionary<IState, List<Transition>> _stateTransitions = new();
        private readonly List<Transition> _globalTransitions = new();

        public void SetInitialState(IState state)
        {
            if (CurrentState == state) return;

            while (_stateStack.Count > 0)
            {
                _stateStack.RemoveAt(_stateStack.Count - 1);
            }

            PushState(state);
        }

        public void AddTransition(IState from, IState to, bool replacing = false, params Func<bool>[] conditions)
        {
            if (!_stateTransitions.TryGetValue(from, out var transitions))
            {
                transitions = new List<Transition>();
                _stateTransitions[from] = transitions;
            }

            transitions.Add(new Transition(to, replacing, conditions));
        }

        public void AddGlobalTransition(IState to, params Func<bool>[] conditions)
        {
            _globalTransitions.Add(new Transition(to, false, conditions));
        }


        public void Tick()
        {
            var transition = GetValidTransition();
            if (transition != null)
            {
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

            _stateStack[^1] = newState;

            EnterNewState(newState);
        }

        private void PushState(IState newState)
        {
            if (newState == CurrentState) return;

            _stateStack.Add(newState);
            EnterNewState(newState);
        }

        private void RemoveState()
        {
            if (_stateStack.Count <= 1) return;

            ExitFromState();

            _stateStack.RemoveAt(_stateStack.Count - 1);

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
            foreach (var transition in _globalTransitions)
                if (transition.CanTransition())
                    return transition;

            if (!_stateTransitions.TryGetValue(CurrentState, out var transitions)) 
                return null;

            foreach (var transition in transitions)
                if (transition.CanTransition())
                    return transition;

            return null;
        }
    }
}