namespace Enemy.Core
{
    [System.Serializable]
    public class Transition
    {
        public DecisionSO decision;
        public State trueState;
        public State falseState;

        public void TryTransition(StateController c) =>
            c.TransitionTo(decision.Decide(c) ? trueState : falseState);
    }
}