using System;
using System.Collections.Generic;

namespace StateMachines.Transition
{
    public sealed class StateMachine : IStateMachine
    {
        public IState CurrentState { get; private set; }

        public event Action<IState> OnStateChanged;

        private readonly Dictionary<IState, List<Transition>> _stateTransitions = new();
        private readonly List<Transition> _globalTransitions = new();

        public void SetInitialState(IState state)
        {
            if (CurrentState == state) return;

            SwitchState(state);
        }

        public void AddTransition(IState from, IState to, params Func<bool>[] conditions)
        {
            if (!_stateTransitions.TryGetValue(from, out var transitions))
            {
                transitions = new List<Transition>();
                _stateTransitions[from] = transitions;
            }

            transitions.Add(new Transition(to, conditions));
        }

        public void AddGlobalTransition(IState to, params Func<bool>[] conditions)
        {
            _globalTransitions.Add(new Transition(to, conditions));
        }

        public void Tick()
        {
            var transition = GetValidTransition();
            if (transition != null)
            {
                SwitchState(transition.To);
            }

            CurrentState?.OnExecute();
        }

        private void SwitchState(IState newState)
        {
            if (newState == CurrentState) return;

            ExitFromState();

            CurrentState = newState;

            EnterNewState(newState);
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