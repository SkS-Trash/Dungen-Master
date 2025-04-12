using UnityEngine;

namespace Enemy
{
    public class FollowState : EnemyState
    {
        private readonly EnemyMovement _movement;
        private readonly Transform _playerTarget;

        public FollowState(EnemyCore core, EnemyMovement movement, Transform playerTarget) : base(core)
        {
            _movement = movement;
            _playerTarget = playerTarget;
        }

        public override void OnEnter()
        {
            _movement.ResumeMoving();
            UpdateDestination();
        }

        public override void OnExecute()
        {
            UpdateDestination();
        }

        private void UpdateDestination()
        {
            if (_playerTarget)
                _movement.MoveTo(_playerTarget.position);
        }
    }
}