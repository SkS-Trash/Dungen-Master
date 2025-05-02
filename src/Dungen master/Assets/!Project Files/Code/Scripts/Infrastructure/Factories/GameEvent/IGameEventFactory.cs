using GameEventObserver;

namespace Factories.GameEvent
{
    public interface IGameEventFactory
    {
        GameEventObserverBehaviour CreateGameEvent(GameEventType eventType, object data = null);
    }
}