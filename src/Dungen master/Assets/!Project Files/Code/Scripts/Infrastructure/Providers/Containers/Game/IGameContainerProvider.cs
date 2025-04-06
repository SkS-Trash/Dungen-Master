namespace Providers.Containers.Game
{
    public interface IGameContainerProvider
    {
        IGameContainer Container { get; }

        void Clear();
    }
}