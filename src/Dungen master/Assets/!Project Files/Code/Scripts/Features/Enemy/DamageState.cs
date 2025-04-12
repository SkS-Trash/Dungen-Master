using UnityEngine;

namespace Enemy
{
    public class DamageState : EnemyState
    {
        private readonly EnemyMovement _movement;
        private readonly EnemyHealth _health;

        public bool IsGetHitEnd => _damageTimer <= 0;

        private const float DAMAGE_ANIMATION_TIME = 0.5f;
        private float _damageTimer;

        public DamageState(EnemyCore core, EnemyMovement movement, EnemyHealth health) : base(core)
        {
            _movement = movement;
            _health = health;
        }

        public override void OnEnter()
        {
            _movement.StopMoving();

            _damageTimer = DAMAGE_ANIMATION_TIME;

            _health.WasDamaged = false;
        }

        public override void OnExecute()
        {
            _damageTimer -= Time.deltaTime;
        }

        public override void OnExit()
        {
            _movement.ResumeMoving();
        }
    }
}