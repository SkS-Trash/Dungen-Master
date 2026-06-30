using UnityEngine;

namespace Enemy.Core
{
    public abstract class State : ScriptableObject
    {
        public ActionSO[] onEnter;
        public ActionSO[] actions;
        public ActionSO[] onExit;
        public Transition[] transitions;

        public virtual void Enter(StateController c)
        {
            foreach (var a in onEnter) a.Act(c);
        }

        public virtual void Exit(StateController c)
        {
            foreach (var a in onExit) a.Act(c);
        }

        public virtual void UpdateState(StateController c)
        {
            foreach (var a in actions) a.Act(c);
            foreach (var t in transitions) t.TryTransition(c);
        }
    }
}