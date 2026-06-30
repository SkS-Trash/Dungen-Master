using GameEventObserver;

namespace Factories.GameEvent
{
    public interface IGameEventFactory
    {
        IGameEventObserver CreateGameEvent(GameEventType eventType, object data = null);
    }
}