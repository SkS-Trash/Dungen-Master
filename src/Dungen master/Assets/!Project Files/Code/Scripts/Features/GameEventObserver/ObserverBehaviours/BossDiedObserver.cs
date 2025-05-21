namespace GameEventObserver
{
    public class BossDiedObserver : GameEventObserverBehaviour,
        IBossDeathEvent
    {
        public override GameEventType EventType => GameEventType.BossDied;

        protected override void Subscribe()
        {
            EventBus.Subscribe(this);
        }

        protected override void Unsubscribe()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnBossDeath() => Notify();
    }
}