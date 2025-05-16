namespace Providers.Containers.Game
{
    public class GameContainerProvider : IGameContainerProvider
    {
        public IGameContainer Container { get; private set; } = new GameContainer();

        public void Clear()
        {
            Container = new GameContainer();
        }
    }
}