namespace Providers.Containers.Scene
{
    public interface ISceneContainerProvider
    {
        ISceneContainer Get();

        void Set(ISceneContainer sceneContainer);

        void Clear();
    }
}