using StateMachines.Transition;

namespace Enemy
{
    public abstract class EnemyState : IState
    {
        protected readonly EnemyCore Core;

        protected EnemyState(EnemyCore core)
        {
            Core = core;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExecute()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}