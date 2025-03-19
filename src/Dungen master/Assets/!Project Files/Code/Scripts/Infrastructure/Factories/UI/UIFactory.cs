using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factories.GameObject;
using Infrastructure.Services.Window;
using UnityEngine;

namespace Infrastructure.Factories.UI
{
    /// <summary>
    /// Фабрика UI.
    /// </summary>
    public class UIFactory : IUIFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly Dictionary<WindowID, UnityEngine.GameObject> _screenInstances = new();

        public UIFactory(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        /// <inheritdoc/>
        public async Task<UnityEngine.GameObject> CreateScreen(string assetAddress, WindowID windowId)
        {
            if (_screenInstances.ContainsKey(windowId))
            {
                Debug.LogWarning($"Экран с WindowID {windowId} уже существует, он будет заменён.");

                DestroyScreen(windowId);
            }

            var instance = await _gameObjectFactory.InstantiateAsync(assetAddress);

            if (_screenInstances.TryAdd(windowId, instance))
            {
                return instance;
            }

            _gameObjectFactory.Destroy(instance);
            return null;
        }

        /// <inheritdoc/>
        public T GetScreenComponent<T>(WindowID windowId) where T : Component
        {
            if (_screenInstances.TryGetValue(windowId, out var screenObject))
            {
                var screenComponent = screenObject.GetComponent<T>();
                if (screenComponent != null)
                {
                    return screenComponent;
                }

                Debug.LogError($"Компонент экрана типа {typeof(T)} не найден");
                return null;
            }

            Debug.LogError($"Экран с WindowID {windowId} не найден");
            return null;
        }

        /// <inheritdoc/>
        public void DestroyScreen(WindowID windowId)
        {
            if (!_screenInstances.Remove(windowId, out var screenObject))
            {
                Debug.LogWarning($"Экран с WindowID {windowId} не найден");
                return;
            }

            _gameObjectFactory.Destroy(screenObject);
        }

        /// <inheritdoc/>
        public bool Exists(WindowID windowId) => _screenInstances.ContainsKey(windowId);
    }
}