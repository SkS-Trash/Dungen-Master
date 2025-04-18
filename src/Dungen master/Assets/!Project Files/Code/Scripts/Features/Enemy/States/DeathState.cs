using UnityEngine;

namespace Enemy
{
    public class DeathState : EnemyState
    {
        private readonly EnemyAnimator _animator;

        public DeathState(EnemyCore core,
            EnemyAnimator animator, EnemyAnimationEvents animationEvents) : base(core)
        {
            _animator = animator;
        }

        public override void OnEnter()
        {
            // Core.GetComponent<Collider>().enabled = false;
            Object.Destroy(Core.gameObject, 2f);
            
            _animator.LaunchDeath();
        }
    }
}