using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface ILaunchContinueGame : IGlobalSubscriber
    {
        void LaunchContinueGame();
    }
}