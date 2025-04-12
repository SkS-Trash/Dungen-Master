using UnityEngine;

namespace Enemy
{
    public class AttackState : EnemyState
    {
        private readonly EnemyMovement _movement;
        private readonly float _attackCooldown;
        private float _lastAttackTime;

        public AttackState(EnemyCore core, EnemyMovement movement, float attackCooldown) : base(core)
        {
            _movement = movement;
            _attackCooldown = attackCooldown;
        }

        public override void OnEnter()
        {
            _movement.StopMoving();
            TryAttack();
        }

        public override void OnExecute()
        {
            if (Time.time - _lastAttackTime >= _attackCooldown)
            {
                TryAttack();
            }
        }

        private void TryAttack()
        {
            Debug.Log("Attacking player!");
            _lastAttackTime = Time.time;
        }
    }
}