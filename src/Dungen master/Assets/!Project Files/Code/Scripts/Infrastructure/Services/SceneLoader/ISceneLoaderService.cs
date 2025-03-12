using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Services
{
    /// <summary>
    /// Интерфейс сервиса для загрузки и выгрузки сцен.
    /// </summary>
    public interface ISceneLoaderService
    {
        /// <summary>
        /// Загружает сцену по ключу.
        /// </summary>
        /// <param name="sceneKey">Ключ сцены.</param>
        /// <param name="loadMode">Режим загрузки сцены (Single или Additive).</param>
        /// <returns>Экземпляр загруженной сцены (SceneInstance).</returns>
        UniTask<SceneInstance> LoadSceneAsync(string sceneKey, LoadSceneMode loadMode = LoadSceneMode.Single);

        /// <summary>
        /// Выгружает ранее загруженную сцену.
        /// </summary>
        /// <param name="sceneInstance">Экземпляр сцены, полученный при загрузке.</param>
        UniTask UnloadSceneAsync(SceneInstance sceneInstance);

        /// <summary>
        /// Выгружает все загруженные сцены.
        /// </summary>
        UniTask UnloadAllScenesAsync();
    }
}