using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Factories.GameObject
{
    /// <summary>
    /// Интерфейс для фабрики, которая создает и управляет GameObject.
    /// </summary>
    public interface IGameObjectFactory
    {
        /// <summary>
        /// Асинхронно создает экземпляр GameObject из указанного пути.
        /// </summary>
        /// <param name="path">Путь к ресурсу GameObject.</param>
        /// <param name="position">Необязательная позиция для размещения GameObject.</param>
        /// <param name="rotation">Необязательная ротация для применения к GameObject.</param>
        /// <param name="parent">Необязательный родительский трансформ для прикрепления GameObject.</param>
        /// <returns>UniTask, который возвращает созданный GameObject.</returns>
        UniTask<UnityEngine.GameObject> InstantiateAsync(
            string path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        );

        /// <summary>
        /// Асинхронно создает экземпляр GameObject из указанного AssetReference.
        /// </summary>
        /// <param name="path">AssetReference к ресурсу GameObject.</param>
        /// <param name="position">Необязательная позиция для размещения GameObject.</param>
        /// <param name="rotation">Необязательная ротация для применения к GameObject.</param>
        /// <param name="parent">Необязательный родительский трансформ для прикрепления GameObject.</param>
        /// <returns>UniTask, который возвращает созданный GameObject.</returns>
        UniTask<UnityEngine.GameObject> InstantiateAsync(
            AssetReference path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        );

        /// <summary>
        /// Асинхронно создает экземпляр GameObject из указанного пути и получает компонент типа T.
        /// </summary>
        /// <typeparam name="T">Тип компонента для получения.</typeparam>
        /// <param name="path">Путь к ресурсу GameObject.</param>
        /// <param name="position">Необязательная позиция для размещения GameObject.</param>
        /// <param name="rotation">Необязательная ротация для применения к GameObject.</param>
        /// <param name="parent">Необязательный родительский трансформ для прикрепления GameObject.</param>
        /// <returns>UniTask, который возвращает компонент типа T.</returns>
        UniTask<T> InstantiateAndGetComponent<T>(
            string path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        ) where T : Component;

        /// <summary>
        /// Асинхронно создает экземпляр GameObject из указанного AssetReference и получает компонент типа T.
        /// </summary>
        /// <typeparam name="T">Тип компонента для получения.</typeparam>
        /// <param name="path">AssetReference к ресурсу GameObject.</param>
        /// <param name="position">Необязательная позиция для размещения GameObject.</param>
        /// <param name="rotation">Необязательная ротация для применения к GameObject.</param>
        /// <param name="parent">Необязательный родительский трансформ для прикрепления GameObject.</param>
        /// <returns>UniTask, который возвращает компонент типа T.</returns>
        UniTask<T> InstantiateAndGetComponent<T>(
            AssetReference path,
            Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null
        ) where T : Component;

        /// <summary>
        /// Уничтожает указанный GameObject.
        /// </summary>
        /// <param name="gameObject">GameObject для уничтожения.</param>
        void Destroy(UnityEngine.GameObject gameObject);
    }
}