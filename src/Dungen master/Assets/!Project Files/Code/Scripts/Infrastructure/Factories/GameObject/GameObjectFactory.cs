using Cysharp.Threading.Tasks;
using Infrastructure.Providers.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Factories.GameObject
{
    /// <summary>
    /// Фабрика для создания игровых объектов.
    /// </summary>
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IObjectResolver _container;

        public GameObjectFactory(
            IAssetsProvider assetsProvider,
            IObjectResolver container
        )
        {
            _assetsProvider = assetsProvider;
            _container = container;
        }

        /// <inheritdoc/>
        public async UniTask<UnityEngine.GameObject> InstantiateAsync(string path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null)
        {
            return InstantiateAsync(await _assetsProvider.GetAsset<UnityEngine.GameObject>(path), position, rotation, parent);
        }

        /// <inheritdoc/>
        public async UniTask<UnityEngine.GameObject> InstantiateAsync(AssetReference path, Vector3? position = null,
            Quaternion? rotation = null, Transform parent = null)
        {
            return InstantiateAsync(await _assetsProvider.GetAsset<UnityEngine.GameObject>(path), position, rotation, parent);
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
        public void Destroy(UnityEngine.GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        private UnityEngine.GameObject InstantiateAsync(UnityEngine.GameObject prefab, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            var instance = _container.Instantiate(
                prefab,
                position ?? Vector3.zero,
                rotation ?? Quaternion.identity,
                parent
            );

            return instance;
        }
    }
}