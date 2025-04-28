using UnityEngine;

namespace Settings.AudioVideoOptionsMenu
{
    public class GameMenuController : MonoBehaviour
    {
        [Header("Should the game pause when opening the menu ?")] [SerializeField]
        private bool pauseOnOpen = true;

        [Space(10)] [SerializeField] private GameObject mainCanvasObj;
        [SerializeField] private GameObject mainMenuPanelObj, optionsPanelObj, graphicsPanelObj, audioPanelObj;
        [SerializeField] private GameObject closeGameImageObj;

        [Space, SerializeField] private bool menuOpenTriggered = false;
        private bool menuOpen;
        private float previousTimescale;

        // Use this for initialization
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            mainCanvasObj.SetActive(false);

            graphicsPanelObj.SetActive(false);
            audioPanelObj.SetActive(false);
            optionsPanelObj.SetActive(false);
            mainMenuPanelObj.SetActive(true);

            closeGameImageObj.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (menuOpenTriggered)
            {
                menuOpenTriggered = false;

                if (menuOpen)
                    ButtonCloseMenu();
                else
                    ButtonOpenMenu();
            }
        }

        public void ButtonOpenMenu()
        {
            if (pauseOnOpen)
            {
                previousTimescale = Time.timeScale; //getting the current timescale
                Time.timeScale = 0; //Pausing time
            }

            graphicsPanelObj.SetActive(false);
            audioPanelObj.SetActive(false);
            optionsPanelObj.SetActive(false);
            mainMenuPanelObj.SetActive(true);

            mainCanvasObj.SetActive(true);

            menuOpen = true;
        }

        public void ButtonCloseMenu()
        {
            if (pauseOnOpen)
            {
                Time.timeScale = previousTimescale; //unpausing time
            }

            mainCanvasObj.SetActive(false);

            graphicsPanelObj.SetActive(false);
            audioPanelObj.SetActive(false);
            optionsPanelObj.SetActive(false);
            mainMenuPanelObj.SetActive(true);

            menuOpen = false;
        }

        public void ButtonQuitGame()
        {
            closeGameImageObj.SetActive(true);
            Application.Quit();
        }

        //for testing/Debugging.
        public void DeleteSavedSettings()
        {
            PlayerPrefs.DeleteKey("Qualitylevel");
            PlayerPrefs.DeleteKey("ResolutionX");
            PlayerPrefs.DeleteKey("ResolutionY");
            PlayerPrefs.DeleteKey("antiAliasSlider");
            PlayerPrefs.DeleteKey("RenderScale");
            PlayerPrefs.DeleteKey("WindowedMode");
            PlayerPrefs.DeleteKey("VSync");
            PlayerPrefs.DeleteKey("AntiAliaslevel");
            PlayerPrefs.DeleteKey("TextureQuality");
            PlayerPrefs.DeleteKey("AnisotropicMode");
            PlayerPrefs.DeleteKey("AnisotropicLevel");

            PlayerPrefs.DeleteKey("muted");
            PlayerPrefs.DeleteKey("mainVolume");
            PlayerPrefs.DeleteKey("fxVolume");
            PlayerPrefs.DeleteKey("musicVolume");
            PlayerPrefs.DeleteKey("speakerMode");
        }
    }
}