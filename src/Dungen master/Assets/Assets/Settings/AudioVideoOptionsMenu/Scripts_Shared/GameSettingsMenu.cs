using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.AudioVideoOptionsMenu
{
    public class GameSettingsMenu : MonoBehaviour
    {
        public enum SaveFormat
        {
            PlayerPrefs,
            IniFile
        }

        [Header("Persistence")] [SerializeField]
        private SaveFormat saveFormat = SaveFormat.PlayerPrefs;

        [SerializeField] private bool usePersistentDataPath = false;

        [Header("UI")] [SerializeField] private Toggle friendlyModeToggle;

        private const string FRIENDLY_KEY = "FriendlyMode";
        private string iniPath;

        private void Awake()
        {
            iniPath = (usePersistentDataPath ? Application.persistentDataPath : Application.dataPath)
                      + "/GameSettings.ini";

            bool saved = LoadFriendlyFlag();
            ApplyFriendlyMode(saved);
            friendlyModeToggle.isOn = saved;
            friendlyModeToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool val)
        {
            ApplyFriendlyMode(val);
            SaveFriendlyFlag(val);
        }

        private static void ApplyFriendlyMode(bool friendly)
        {
            GameMode.IsFriendly = friendly;
        }

        private void SaveFriendlyFlag(bool friendly)
        {
            switch (saveFormat)
            {
                case SaveFormat.PlayerPrefs:
                    PlayerPrefs.SetInt(FRIENDLY_KEY, friendly ? 1 : 0);
                    break;

                case SaveFormat.IniFile:
                    File.WriteAllText(iniPath, friendly ? "true" : "false");
                    break;
            }
        }

        private bool LoadFriendlyFlag()
        {
            switch (saveFormat)
            {
                case SaveFormat.PlayerPrefs:
                    return PlayerPrefs.GetInt(FRIENDLY_KEY, 0) == 1;

                case SaveFormat.IniFile:
                    return File.Exists(iniPath) && bool.TryParse(File.ReadAllText(iniPath), out var val) && val;

                default:
                    return false;
            }
        }

        private void OnDisable() => SaveFriendlyFlag(friendlyModeToggle.isOn);
    }

    public static class GameMode
    {
        public static bool IsFriendly = false;
    }
}