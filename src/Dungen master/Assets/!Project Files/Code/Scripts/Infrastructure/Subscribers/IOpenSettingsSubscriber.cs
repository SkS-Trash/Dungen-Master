using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface IOpenSettingsSubscriber : IGlobalSubscriber
    {
        void OpenSettings();
    }
}