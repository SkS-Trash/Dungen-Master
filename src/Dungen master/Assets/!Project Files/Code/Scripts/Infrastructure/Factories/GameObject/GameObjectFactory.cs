using Cysharp.Threading.Tasks;
using Providers;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Factories
{
    /// <summary>
    /// Фабрика для создания игровых объектов.
    /// </summary>
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly Container _container;
        private readonly IAssetsProvider _assetsProvider;

        public GameObjectFactory(
            Container container,
            IAssetsProvider assetsProvider
        )
        {
            _container = container;
            _assetsProvider = assetsProvider;
        }

        /// <inheritdoc/>
        public async UniTask<GameObject> InstantiateAsync(string path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null)
        {
            return InstantiateAsync(await _assetsProvider.GetAsset<GameObject>(path), position, rotation, parent);
        }

        /// <inheritdoc/>
        public async UniTask<GameObject> InstantiateAsync(AssetReference path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null)
        {
            return InstantiateAsync(await _assetsProvider.GetAsset<GameObject>(path), position, rotation, parent);
        }

        /// <inheritdoc/>
        public async UniTask<T> InstantiateAndGetComponent<T>(string path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null) where T : Component
        {
            return (await InstantiateAsync(path, position, rotation, parent)).GetComponent<T>();
        }

        /// <inheritdoc/>
        public async UniTask<T> InstantiateAndGetComponent<T>(AssetReference path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null) where T : Component
        {
            return (await InstantiateAsync(path, position, rotation, parent)).GetComponent<T>();
        }

        /// <inheritdoc/>
        public void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        private GameObject InstantiateAsync(GameObject prefab, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            var instance = Object.Instantiate(
                prefab,
                position ?? Vector3.zero,
                rotation ?? Quaternion.identity,
                parent
            );

            GameObjectInjector.InjectRecursive(instance, _container);

            return instance;
        }
    }
}