using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.SceneLoader
{
    /// <summary>
    /// Сервис для загрузки и выгрузки сцен с использованием Addressables.
    /// </summary>
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly List<SceneInstance> _loadedScenes = new();

        /// <inheritdoc/>
        public async UniTask<SceneInstance> LoadSceneAsync(
            string sceneKey,
            LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            bool saveInCache = true
        )
        {
            var loadHandle = Addressables.LoadSceneAsync(sceneKey, loadMode, activateOnLoad);

            await loadHandle.Task;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var sceneInstance = loadHandle.Result;

                if (saveInCache)
                {
                    _loadedScenes.Add(sceneInstance);
                }

                Debug.Log($"Сцена '{sceneInstance.Scene.name}' успешно загружена.");

                return sceneInstance;
            }

            Debug.LogError($"Не удалось загрузить сцену '{sceneKey}'. Статус: {loadHandle.Status}");

            return default;
        }

        /// <inheritdoc/>
        public async UniTask UnloadSceneAsync(SceneInstance sceneInstance)
        {
            var unloadHandle = Addressables.UnloadSceneAsync(sceneInstance);

            await unloadHandle.Task;

            if (unloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedScenes.Remove(sceneInstance);

                Debug.Log($"Сцена '{sceneInstance.Scene.name}' успешно выгружена.");
                return;
            }

            Debug.LogError($"Не удалось выгрузить сцену '{sceneInstance.Scene.name}'. Статус: {unloadHandle.Status}");
        }

        /// <inheritdoc/>
        public async UniTask UnloadAllScenesAsync()
        {
            foreach (var sceneInstance in _loadedScenes)
            {
                await UnloadSceneAsync(sceneInstance);
            }

            _loadedScenes.Clear();

            Debug.Log("Все сцены успешно выгружены.");
        }
    }
}