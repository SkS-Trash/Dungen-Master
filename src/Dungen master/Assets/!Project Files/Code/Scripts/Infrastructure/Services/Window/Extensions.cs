namespace Services.Window
{
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