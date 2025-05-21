namespace GameEventObserver
{
    public class EnemyDiedObserver : GameEventObserverBehaviour,
        IEnemyDeathEvent
    {
        public override GameEventType EventType => GameEventType.EnemyDied;

        protected override void Subscribe()
        {
            EventBus.Subscribe(this);
        }

        protected override void Unsubscribe()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnEnemyDeath() => Notify();
    }
}