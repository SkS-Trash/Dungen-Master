using UnityEngine;
using Weapon;

namespace Enemy
{
    public class AttackState : EnemyState
    {
        private readonly EnemyAnimator _animator;
        private readonly EnemyAnimationEvents _animationEvents;
        private readonly WeaponInHandController _weapon;
        private readonly Transform _playerTransform;
        private readonly float _attackCooldown;

        public bool IsAttackEnd { get; private set; }
        public bool IsAttackReady => Time.time >= _attackReadyTime;

        private float _attackReadyTime;

        public AttackState(
            EnemyCore core,
            EnemyAnimator animator,
            EnemyAnimationEvents animationEvents,
            float attackCooldown,
            WeaponInHandController weapon,
            Transform playerTransform
        ) : base(core)
        {
            _animator = animator;
            _animationEvents = animationEvents;
            _attackCooldown = attackCooldown;
            _weapon = weapon;
            _playerTransform = playerTransform;
        }

        public override void OnEnter()
        {
            IsAttackEnd = false;

            _animator.LaunchAttack();
            _animationEvents.OnAnimationEvent += OnAnimationEvent;
        }

        public override void OnExecute()
        {
            TurnTowardsPlayer();
        }

        public override void OnExit()
        {
            _attackReadyTime = Time.time + _attackCooldown;
            _animationEvents.OnAnimationEvent -= OnAnimationEvent;
        }

        private void TurnTowardsPlayer()
        {
            if (_playerTransform == null) return;

            var direction = _playerTransform.position - Core.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                Core.transform.rotation = Quaternion.Slerp(Core.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        private void OnAnimationEvent(EnemyAnimationEvents.AnimationEventType eventType)
        {
            switch (eventType)
            {
                case EnemyAnimationEvents.AnimationEventType.AttackEnd:
                    IsAttackEnd = true;
                    break;
                case EnemyAnimationEvents.AnimationEventType.EnablePhysicalAttack:
                    _weapon?.EnableWeaponCollider();
                    break;
                case EnemyAnimationEvents.AnimationEventType.DisablePhysicalAttack:
                    _weapon?.DisableWeaponCollider();
                    break;
                case EnemyAnimationEvents.AnimationEventType.LaunchMagicAttack:
                    break;
            }
        }
    }
}