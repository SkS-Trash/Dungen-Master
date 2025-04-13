using UnityEngine;

namespace Enemy
{
    public class FollowState : EnemyState
    {
        private readonly EnemyMovement _movement;
        private readonly EnemyAnimator _animator;
        private readonly float _chaseSpeed;
        private readonly Transform _playerTarget;

        public FollowState(EnemyCore core,
            EnemyMovement movement,
            EnemyAnimator animator,
            float chaseSpeed,
            Transform playerTarget
        ) : base(core)
        {
            _movement = movement;
            _animator = animator;
            _chaseSpeed = chaseSpeed;
            _playerTarget = playerTarget;
        }

        public override void OnEnter()
        {
            _movement.ResumeMoving();
            _animator.SetIsRun(true);

            _movement.SetSpeed(_chaseSpeed);
            _movement.SetStoppingDistance(1f, true);
        }

        public override void OnExecute()
        {
            UpdateDestination();
        }

        public override void OnExit()
        {
            _movement.StopMoving();
            _animator.SetIsRun(false);
        }

        private void UpdateDestination()
        {
            if (_playerTarget)
                _movement.MoveTo(_playerTarget.position);
        }
    }
}