using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface ISettingsButton : IGlobalSubscriber
    {
        void OnSettingsButtonClicked();
    }
}