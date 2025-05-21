namespace GameEventObserver
{
    public interface IGameEventObserver
    {
        GameEventType EventType { get; }
        
        void Register(GameEvent @event);
        void Unregister(GameEvent @event);
        void UnregisterAll();
    }
}