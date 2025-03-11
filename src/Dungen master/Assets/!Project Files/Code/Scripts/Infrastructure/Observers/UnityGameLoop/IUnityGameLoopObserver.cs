using System;

namespace Observers
{
    /// <summary>
    /// Интерфейс для наблюдателей за игровым циклом Unity.
    /// </summary>
    public interface IUnityGameLoopObserver
    {
        /// <summary>
        /// Событие, вызываемое при изменении фокуса приложения.
        /// </summary>
        public event Action<bool> ApplicationFocusChanged;

        /// <summary>
        /// Событие, вызываемое при изменении состояния паузы приложения.
        /// </summary>
        public event Action<bool> ApplicationPauseChanged;

        /// <summary>
        /// Событие, вызываемое при выходе из приложения.
        /// </summary>
        public event Action ApplicationQuit;

        /// <summary>
        /// Событие, вызываемое при обновлении приложения.
        /// </summary>
        public event Action ApplicationUpdate;
    }
}