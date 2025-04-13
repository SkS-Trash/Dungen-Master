using UnityEngine;

namespace Enemy
{
    public class AttackState : EnemyState
    {
        private readonly EnemyAnimator _animator;
        private readonly EnemyAnimationEvents _animationEvents;
        private readonly float _attackCooldown;

        public bool IsAttackEnd { get; private set; }
        public bool IsAttackReady => Time.time >= _lastAttackTime + _attackCooldown;

        private float _lastAttackTime;

        public AttackState(EnemyCore core,
            EnemyAnimator animator,
            EnemyAnimationEvents animationEvents,
            float attackCooldown
        ) : base(core)
        {
            _animator = animator;
            _animationEvents = animationEvents;
            _attackCooldown = attackCooldown;
        }

        public override void OnEnter()
        {
            IsAttackEnd = false;
            
            _animator.LaunchAttack();
            _animationEvents.OnAnimationEvent += OnAnimationEvent;
        }

        public override void OnExit()
        {
            _lastAttackTime = Time.time;
            _animationEvents.OnAnimationEvent -= OnAnimationEvent;
        }

        private void OnAnimationEvent(EnemyAnimationEvents.AnimationEventType eventType)
        {
            switch (eventType)
            {
                case EnemyAnimationEvents.AnimationEventType.AttackEnd:
                    IsAttackEnd = true;
                    break;
                case EnemyAnimationEvents.AnimationEventType.EnablePhysicalAttack:
                case EnemyAnimationEvents.AnimationEventType.DisablePhysicalAttack:
                case EnemyAnimationEvents.AnimationEventType.LaunchMagicAttack:
                    Debug.Log($"Animation event: {eventType}");
                    break;
            }
        }
    }
}