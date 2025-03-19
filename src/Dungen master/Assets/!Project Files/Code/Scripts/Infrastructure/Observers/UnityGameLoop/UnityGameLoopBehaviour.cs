using System;
using UnityEngine;

namespace Infrastructure.Observers.UnityGameLoop
{
    /// <summary>
    /// Компонент, реализующий интерфейс <see cref="IUnityGameLoopObserver"/> для наблюдения за игровым циклом Unity.
    /// </summary>
    public class UnityGameLoopBehaviour : MonoBehaviour, IUnityGameLoopObserver
    {
        /// <inheritdoc/>
        public event Action<bool> ApplicationFocusChanged;

        /// <inheritdoc/>
        public event Action<bool> ApplicationPauseChanged;

        /// <inheritdoc/>
        public event Action ApplicationQuit;

        /// <inheritdoc/>
        public event Action ApplicationUpdate;

        private void OnApplicationFocus(bool hasFocus)
        {
            ApplicationFocusChanged?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            ApplicationPauseChanged?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            ApplicationQuit?.Invoke();
        }

        private void Update()
        {
            ApplicationUpdate?.Invoke();
        }
    }
}