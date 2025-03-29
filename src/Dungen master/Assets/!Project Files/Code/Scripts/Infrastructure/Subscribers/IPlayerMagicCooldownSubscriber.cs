using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface IPlayerMagicCooldownSubscriber : IGlobalSubscriber
    {
        void OnPlayerMagicCooldownChanged(float percentage);
    }
}