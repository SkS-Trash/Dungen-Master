using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface IPlayerDeathSubscriber : IGlobalSubscriber
    {
        void OnPlayerDeath();
    }
}