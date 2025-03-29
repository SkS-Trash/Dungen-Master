namespace Providers.Containers.Scene
{
    /// <summary>
    /// Обеспечивает контейнер сцены.
    /// </summary>
    public class SceneContainerProvider : ISceneContainerProvider
    {
        private SceneContainer _sceneContainer;

        /// <inheritdoc/>
        public SceneContainer GetSceneContainer()
        {
            return _sceneContainer;
        }

        /// <inheritdoc/>
        public void SetSceneContainer(SceneContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
        }
    }
}