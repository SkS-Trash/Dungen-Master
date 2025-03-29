using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface IPlayerHealthPercentageSubscriber : IGlobalSubscriber
    {
        void OnPlayerHealthPercentageChanged(float percentage);
    }
}