namespace Enemy
{
    public class IdleState : EnemyState
    {
        private readonly EnemyMovement _movement;

        public IdleState(EnemyCore core, EnemyMovement movement) : base(core)
        {
            _movement = movement;
        }

        public override void OnEnter()
        {
            _movement.StopMoving();
        }
    }
}