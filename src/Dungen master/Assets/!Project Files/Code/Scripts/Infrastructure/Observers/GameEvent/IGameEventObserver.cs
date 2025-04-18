using Subscribers;

namespace Observers.GameEvent
{
    public interface IGameEventObserver :
        IPlayerDeathSubscriber
    {
    }
}