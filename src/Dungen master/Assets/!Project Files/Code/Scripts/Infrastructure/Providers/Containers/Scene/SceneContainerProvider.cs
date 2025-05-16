namespace Providers.Containers.Scene
{
    public class SceneContainerProvider : ISceneContainerProvider
    {
        private ISceneContainer _sceneContainer;

        public ISceneContainer Get()
        {
            return _sceneContainer;
        }

        public void Set(ISceneContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
        }

        public void Clear()
        {
            _sceneContainer = null;
        }
    }
}