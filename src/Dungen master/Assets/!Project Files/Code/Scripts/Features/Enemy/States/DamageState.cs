namespace Enemy
{
    public class DamageState : EnemyState
    {
        private readonly EnemyAnimator _animator;
        private readonly EnemyAnimationEvents _animationEvents;
        private readonly EnemyHealth _health;

        public bool IsGetHitEnd { get; private set; }

        public DamageState(EnemyCore core,
            EnemyAnimator animator,
            EnemyAnimationEvents animationEvents,
            EnemyHealth health
        ) : base(core)
        {
            _animator = animator;
            _animationEvents = animationEvents;
            _health = health;
        }

        public override void OnEnter()
        {
            IsGetHitEnd = false;
            _health.WasDamaged = false;
            _animator.LaunchHit();
            _animationEvents.OnAnimationEvent += OnAnimationEvent;
        }

        public override void OnExit()
        {
            _animationEvents.OnAnimationEvent -= OnAnimationEvent;
        }

        private void OnAnimationEvent(EnemyAnimationEvents.AnimationEventType eventType)
        {
            if (eventType == EnemyAnimationEvents.AnimationEventType.GetHitEnd)
            {
                IsGetHitEnd = true;
            }
        }
    }
}