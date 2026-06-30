using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Observers.UnityGameLoop
{
    /// <summary>
    /// Класс, реализующий интерфейс <see cref="IUnityGameLoopObserver"/> для наблюдения за игровым циклом Unity.
    /// </summary>
    public class UnityGameLoopObserver : IUnityGameLoopObserver
    {
        /// <inheritdoc/>
        public event Action<bool> ApplicationFocusChanged;

        /// <inheritdoc/>
        public event Action<bool> ApplicationPauseChanged;

        /// <inheritdoc/>
        public event Action ApplicationQuit;

        /// <inheritdoc/>
        public event Action ApplicationUpdate;

        public UnityGameLoopObserver()
        {
            var gameObject = new GameObject("UnityGameLoopBehaviour");
            var unityGameLoopBehaviour = gameObject.AddComponent<UnityGameLoopBehaviour>();
            Object.DontDestroyOnLoad(gameObject);

            unityGameLoopBehaviour.ApplicationFocusChanged += OnApplicationFocusChanged;
            unityGameLoopBehaviour.ApplicationPauseChanged += OnApplicationPauseChanged;
            unityGameLoopBehaviour.ApplicationQuit += OnApplicationQuit;
            unityGameLoopBehaviour.ApplicationUpdate += OnApplicationUpdate;
        }

        private void OnApplicationFocusChanged(bool hasFocus)
        {
            ApplicationFocusChanged?.Invoke(hasFocus);
        }

        private void OnApplicationPauseChanged(bool pauseStatus)
        {
            ApplicationPauseChanged?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            ApplicationQuit?.Invoke();
        }

        private void OnApplicationUpdate()
        {
            ApplicationUpdate?.Invoke();
        }
    }
}