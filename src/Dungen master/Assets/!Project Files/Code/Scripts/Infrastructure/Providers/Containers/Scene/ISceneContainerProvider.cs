namespace Providers.Containers.Scene
{
    /// <summary>
    /// Интерфейс для предоставления контейнера сцены.
    /// </summary>
    public interface ISceneContainerProvider
    {
        /// <summary>
        /// Получает контейнер сцены.
        /// </summary>
        /// <returns>Контейнер сцены.</returns>
        SceneContainer GetSceneContainer();

        /// <summary>
        /// Устанавливает контейнер сцены.
        /// </summary>
        /// <param name="sceneContainer">Контейнер сцены для установки.</param>
        void SetSceneContainer(SceneContainer sceneContainer);
    }
}