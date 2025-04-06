using UnityEngine;

namespace Enemy
{
    public class PatrolState : EnemyState
    {
        private readonly EnemyMovement _movement;

        public PatrolState(EnemyCore core, EnemyMovement movement) : base(core)
        {
            _movement = movement;
        }

        public override void OnEnter()
        {
            MoveToNextPoint();
        }

        public override void OnExecute()
        {
            if (_movement.HasReachedDestination())
            {
                MoveToNextPoint();
            }
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