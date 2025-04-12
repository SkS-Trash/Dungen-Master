using UnityEngine;

namespace Enemy
{
    public class DeathState : EnemyState
    {
        private readonly EnemyMovement _movement;

        public DeathState(EnemyCore core, EnemyMovement movement) : base(core)
        {
            _movement = movement;
        }

        public override void OnEnter()
        {
            _movement.StopMoving();
            // Core.GetComponent<Collider>().enabled = false;
            Object.Destroy(Core.gameObject, 2f);
        }
    }
}