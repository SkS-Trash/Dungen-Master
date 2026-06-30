namespace GameEventObserver
{
    public class PlayerDiedObserver : GameEventObserverBehaviour,
        IPlayerDeathEvent
    {
        public override GameEventType EventType => GameEventType.PlayerDied;
        
        protected override void Subscribe()
        {
            EventBus.Subscribe(this);
        }

        protected override void Unsubscribe()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnPlayerDeath() => Notify();
    }
}