using System.Linq;

namespace Enemy.Core
{
    [System.Serializable]
    public class Transition
    {
        public DecisionSO[] decisions;
        public State trueState;
        public State falseState;

        public void TryTransition(StateController c)
        {
            if (decisions.Length == 0) return;

            var decisionResult = decisions.All(d => d.Decide(c));
            switch (decisionResult)
            {
                case true when trueState:
                    c.TransitionTo(trueState);
                    return;
                case false when falseState:
                    c.TransitionTo(falseState);
                    return;
            }
        }
    }
}