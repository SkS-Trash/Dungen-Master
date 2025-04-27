using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface IQuitApplication : IGlobalSubscriber
    {
        void QuitApplication();
    }
}