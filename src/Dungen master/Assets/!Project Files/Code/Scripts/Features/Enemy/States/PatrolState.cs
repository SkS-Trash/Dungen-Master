using UnityEngine;

namespace Enemy
{
    public class PatrolState : EnemyState
    {
        private readonly EnemyMovement _movement;
        private readonly EnemyAnimator _animator;
        private readonly float _patrolSpeed;

        public PatrolState(EnemyCore core,
            EnemyMovement movement,
            EnemyAnimator animator,
            float patrolSpeed
        ) : base(core)
        {
            _movement = movement;
            _animator = animator;
            _patrolSpeed = patrolSpeed;
        }

        public override void OnEnter()
        {
            MoveToNextPoint();

            _movement.ResumeMoving();
            _animator.SetIsWalk(true);

            _movement.SetSpeed(_patrolSpeed);
            _movement.SetStoppingDistance(0f, false);
        }

        public override void OnExecute()
        {
            if (_movement.HasReachedDestination())
            {
                MoveToNextPoint();
            }
        }

        public override void OnExit()
        {
            _movement.StopMoving();
            _animator.SetIsWalk(false);
        }

        private void MoveToNextPoint()
        {
            var nextPoint = GetNextPoint();
            _movement.MoveTo(nextPoint);
        }

        private Vector3 GetNextPoint()
        {
            var position = Core.transform.position;

            var forward = Core.transform.forward * Random.Range(1.5f, 3.5f);
            if (Random.Range(0, 2) == 0)
                forward *= -1;

            var right = Core.transform.right * Random.Range(0.5f, 2.5f);
            if (Random.Range(0, 2) == 0)
                right *= -1;

            var targetPosition = position + forward + right;
            targetPosition.y = position.y;

            return targetPosition;
        }
    }
}