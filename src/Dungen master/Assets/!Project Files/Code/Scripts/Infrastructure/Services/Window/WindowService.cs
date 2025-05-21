using Cysharp.Threading.Tasks;
using Factories.UI;
using UnityEngine;

namespace Services.Window
{
    /// <summary>
    /// Сервис для управления окнами в приложении.
    /// </summary>
    public class WindowService : IWindowService
    {
        public WindowService(
            IUIFactory uiFactory
        )
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        /// <inheritdoc/>
        public bool IsWindowOpened(WindowID windowID) =>
            _uiFactory.Exists(windowID);

        /// <inheritdoc/>
        public async UniTask<GameObject> Open(WindowID windowID)
        {
            var windowsPath = windowID.GetWindowsPath();

            return await _uiFactory.CreateScreen(windowsPath, windowID);
        }

        /// <inheritdoc/>
        public async UniTask<T> OpenAndGet<T>(WindowID windowID) where T : Component
        {
            await Open(windowID);

            return _uiFactory.GetScreenComponent<T>(windowID);
        }

        /// <inheritdoc/>
        public T Get<T>(WindowID windowID) where T : Component =>
            _uiFactory.GetScreenComponent<T>(windowID);

        /// <inheritdoc/>
        public void Close(WindowID windowID) =>
            _uiFactory.DestroyScreen(windowID);
    }
}