using Subscribers.EventBusSystem;

namespace Subscribers
{
    public interface ILaunchNewGame : IGlobalSubscriber
    {
        void LaunchNewGame();
    }
}