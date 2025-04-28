using Subscribers.EventBusSystem;

namespace UI.Settings
{
    public interface ICloseSettingsSubscriber : IGlobalSubscriber
    {
        void CloseSettings();
    }
}