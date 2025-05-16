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
        public async UniTask Open(WindowID windowID)
        {
            var windowsPath = windowID.GetWindowsPath();

            await _uiFactory.CreateScreen(windowsPath, windowID);
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

    internal static class Extensions
    {
        internal static string GetWindowsPath(this WindowID windowID) => windowID switch
        {
            WindowID.GameLoading => WindowsPaths.GAME_LOADING_PATH,
            WindowID.MainMenu => WindowsPaths.MAIN_MENU_PATH,
            WindowID.Settings => WindowsPaths.SETTINGS_PATH,

            WindowID.HomePauseMenu => WindowsPaths.HOME_PAUSE_MENU_PATH,

            WindowID.GamePauseMenu => WindowsPaths.GAME_PAUSE_MENU_PATH,
            WindowID.Game => WindowsPaths.GAME_PATH,
            WindowID.HUD => WindowsPaths.HUD_PATH,

            WindowID.GameOver => WindowsPaths.GAME_OVER_PATH,
            WindowID.Victory => WindowsPaths.VICTORY_PATH,
            _ => null
        };
    }
}