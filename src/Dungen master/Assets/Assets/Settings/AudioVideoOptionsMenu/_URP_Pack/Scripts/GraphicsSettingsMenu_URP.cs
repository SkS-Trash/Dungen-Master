using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Settings.AudioVideoOptionsMenu
{
    public class GraphicsSettingsMenu_URP : MonoBehaviour
{
    public enum SaveFormat { playerprefs, iniFile };
    [Space(10)]
    public SaveFormat saveFormat;

    [Header("Set True for IOS or Windows Store Apps.")]
    public bool usePersistentDatapath; //Use Persistent for "IOS" and "Windows Store Apps" or if you prefer to saves the file in a seperate persistent Folder.

    [Header("Select wich settings you want to use. Settings set to[UnUsed] can be removed from/disabled in the menu UI")]
    [Space(10)]
    [SerializeField]
    private SettingsUsedState qualityLevelUsed;
    [SerializeField]
    private SettingsUsedState resolutionUsed, renderScaleUsed, windowedModeUsed, vSyncUsed, antiAliasingUsed, textureQualityUsed, anisotropicModeUsed, anisotropicLvlUsed;

    [Header("Values to use on Reset or if no values are saved")]
    [SerializeField]
    private MenuVariables_URP DefaultSettings = new MenuVariables_URP();

    private MenuVariablesSimple_URP DefaultSettingsConverted = new MenuVariablesSimple_URP();
    private MenuVariablesSimple_URP CurrentSettings = new MenuVariablesSimple_URP();

    [Header("UI elements references")]
    [Space(10)]
    [SerializeField]
    private TMP_Text qualityLevelText;
    [SerializeField]
    private TMP_Text resolutionText, windowedModeText, renderScaleText, vsyncText, antiAliasingText, textureQualityText, anisoFilteringModeText, anisoFilteringLevelText;
    [SerializeField]
    private Slider renderScaleSlider;
    [SerializeField]
    private GameObject anisoLevelObj;

    private string saveFileDataPath;
    private List<Resolution> availableResolutions = new List<Resolution>();
    private int currentResolutionIndex;

    private bool initiated, isApplying;

    private void Awake()
    {
#if UNITY_EDITOR
        if (UnityEngine.EventSystems.EventSystem.current == null)
            Debug.LogWarning("There is no Event System in the scene !! UI Elements can not detect input.");
#endif
        //Use Persistent for "IOS" and "Windows Store Apps" or if you prefer to saves the file in a seperate persistent Folder.
        if (!usePersistentDatapath)
            saveFileDataPath = Application.dataPath + "/QualitySettings.ini";//puts the file in the games/applications folder.
        else saveFileDataPath = Application.persistentDataPath + "/QualitySettings.ini";

        //get available resolutions and filter them.
        Resolution[] availableResolutionsAll = Screen.resolutions;//checking the available resolution options.

        //we get every resolution with every available refreshrate, we only need the resolution ones.
        float resX = 0, resY= 0;
        for (int i = 0; i < availableResolutionsAll.Length; i++)
        {
            if (resX != availableResolutionsAll[i].width && resY != availableResolutionsAll[i].height)
            {
                resX = availableResolutionsAll[i].width;
                resY = availableResolutionsAll[i].height;

                availableResolutions.Add(availableResolutionsAll[i]);
            }
        }
        availableResolutionsAll = null;

        ConvertDefaultSettings();
        LoadMenuVariables();
        initiated = true;
    }

    //converting the easier to read settings class to the easyer to use in script settings class
    private void ConvertDefaultSettings()
    {
        DefaultSettingsConverted.Qualitylevel = DefaultSettings.Qualitylevel;

        if (DefaultSettings.Resolution.x == 0 || DefaultSettings.Resolution.y == 0)
        {
            DefaultSettingsConverted.ResolutionX = Screen.width;
            DefaultSettingsConverted.ResolutionY = Screen.height;
        }
        else
        {
            DefaultSettingsConverted.ResolutionX = DefaultSettings.Resolution.x;
            DefaultSettingsConverted.ResolutionY = DefaultSettings.Resolution.y;
        }

        DefaultSettingsConverted.RenderScale = DefaultSettings.RenderScale;

        switch (DefaultSettings.WindowedMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                DefaultSettingsConverted.WindowedMode = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                DefaultSettingsConverted.WindowedMode = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                DefaultSettingsConverted.WindowedMode = 2;
                break;
            case FullScreenMode.Windowed:
                DefaultSettingsConverted.WindowedMode = 3;
                break;
        }

        switch (DefaultSettings.VSync)
        {
            case VSyncEnum.off:
                DefaultSettingsConverted.VSync = 0;
                break;
            case VSyncEnum.full:
                DefaultSettingsConverted.VSync = 1;
                break;
            case VSyncEnum.half:
                DefaultSettingsConverted.VSync = 2;
                break;
        }

        switch (DefaultSettings.AntiAliaslevel)
        {
            case AntiAliasLevelEnum.off:
                DefaultSettingsConverted.AntiAliaslevel = 0;
                break;
            case AntiAliasLevelEnum.x2:
                DefaultSettingsConverted.AntiAliaslevel = 2;
                break;
            case AntiAliasLevelEnum.x4:
                DefaultSettingsConverted.AntiAliaslevel = 4;
                break;
            case AntiAliasLevelEnum.x8:
                DefaultSettingsConverted.AntiAliaslevel = 8;
                break;
        }

        switch (DefaultSettings.TextureQuality)
        {
            case TextureQualityEnum.FullRes:
                DefaultSettingsConverted.TextureQuality = 0;
                break;
            case TextureQualityEnum.HalfRes:
                DefaultSettingsConverted.TextureQuality = 1;
                break;
            case TextureQualityEnum.QuarterRes:
                DefaultSettingsConverted.TextureQuality = 2;
                break;
            case TextureQualityEnum.EighthRes:
                DefaultSettingsConverted.TextureQuality = 3;
                break;
        }

        switch (DefaultSettings.AnisotropicMode)
        {
            case AnisotropicFiltering.Disable:
                DefaultSettingsConverted.AnisotropicMode = 0;
                break;
            case AnisotropicFiltering.Enable:
                DefaultSettingsConverted.AnisotropicMode = 1;
                break;
            case AnisotropicFiltering.ForceEnable:
                DefaultSettingsConverted.AnisotropicMode = 2;
                break;
        }

        switch (DefaultSettings.AnisotropicLevel)
        {
            case AnisotropicLevelEnum.x2:
                DefaultSettingsConverted.AnisotropicLevel = 2;
                break;
            case AnisotropicLevelEnum.x4:
                DefaultSettingsConverted.AnisotropicLevel = 4;
                break;
            case AnisotropicLevelEnum.x8:
                DefaultSettingsConverted.AnisotropicLevel = 8;
                break;
            case AnisotropicLevelEnum.x16:
                DefaultSettingsConverted.AnisotropicLevel = 16;
                break;
            default:
                break;
        }

        DefaultSettingsConverted.Warning = DefaultSettings.WarningMessage;
        CurrentSettings.Warning = DefaultSettings.WarningMessage;
    }

    #region Button functions
    public void UI_SetQualityLevel(int _addSubtract) //changes the general Quality setting without changing the Vsync,Antialias or Anisotropic settings.
    {
        if (qualityLevelUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.Qualitylevel += _addSubtract;
        CurrentSettings.Qualitylevel = Mathf.Clamp(CurrentSettings.Qualitylevel, 0, QualitySettings.names.Length-1);

        if (CurrentSettings.Qualitylevel != QualitySettings.GetQualityLevel())
        {
            //Changing Quality Levels overrides all changed settings.
            //We have to apply all of them again.
            ApplySettings(CurrentSettings);
        }
    }

    public void UI_SetResolution(int _addSubtract)
    {
        if (resolutionUsed == SettingsUsedState.notUsed || isApplying)
            return;

        currentResolutionIndex += _addSubtract;
    
        //loop around
        if (currentResolutionIndex < 0)
            currentResolutionIndex = availableResolutions.Count - 1;
        else if (currentResolutionIndex >= availableResolutions.Count)
            currentResolutionIndex = 0;

        CurrentSettings.ResolutionX = availableResolutions[currentResolutionIndex].width;
        CurrentSettings.ResolutionY = availableResolutions[currentResolutionIndex].height;

        //can`t change resolution without setting FullScreenMode.
        switch (CurrentSettings.WindowedMode)
        {
            case 0:
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.ExclusiveFullScreen);
                break;                                                                 
            case 1:                                                                    
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.FullScreenWindow);
                break;
            case 2:
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.MaximizedWindow);
                break;
            case 3:
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.Windowed);
                break;
        }

        resolutionText.SetText("{0}x{1}", CurrentSettings.ResolutionX, CurrentSettings.ResolutionY);
    }

    public void UI_SetRenderScale(Slider _slider)
    {
        if (renderScaleUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.RenderScale = _slider.value/ 10;

        UniversalRenderPipelineAsset URPAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
        URPAsset.renderScale = CurrentSettings.RenderScale;
        URPAsset = null;

        renderScaleText.text = CurrentSettings.RenderScale.ToString();
    }

    public void UI_SetWindowedMode(int _windowedMode)
    {
        if (windowedModeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.WindowedMode = _windowedMode;

        switch (CurrentSettings.WindowedMode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                windowedModeText.text = "FullScreen";
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                windowedModeText.text = "FullScreen Windowed";
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                windowedModeText.text = "Maximized Windowed";
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                windowedModeText.text = "Windowed";
                break;
        }
    }

    public void UI_SetVSync(int _vSync)
    {
        if (vSyncUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.VSync = _vSync;

        QualitySettings.vSyncCount = CurrentSettings.VSync;

        switch (_vSync)
        {
            case 0:
                vsyncText.text = "Off";
                break;
            case 1:
                vsyncText.text = "Full";
                break;
            case 2:
                vsyncText.text = "Half";
                break;
        }
    }

    public void UI_SetAntiAliasing(int _antiAliaslevel)
    {
        if (antiAliasingUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.AntiAliaslevel = _antiAliaslevel;

        UniversalRenderPipelineAsset URPAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
        URPAsset.msaaSampleCount = CurrentSettings.AntiAliaslevel;

        if (CurrentSettings.AntiAliaslevel != 0)
            antiAliasingText.SetText("{0}x", CurrentSettings.AntiAliaslevel);
        else antiAliasingText.text = "Off";

        URPAsset = null;
    }

    public void UI_SetTextureQuality(int _textureQuality)
    {
        if (textureQualityUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.TextureQuality = _textureQuality;
        QualitySettings.globalTextureMipmapLimit = CurrentSettings.TextureQuality;

        switch (CurrentSettings.TextureQuality)
        {
            case 0:
                textureQualityText.text = "Full";
                break;
            case 1:
                textureQualityText.text = "Half";
                break;
            case 2:
                textureQualityText.text = "Quarte";
                break;
            case 3:
                textureQualityText.text = "Eighth";
                break;
        }
    }

    public void UI_SetAnisotropicMode(int _anisotropicMode)
    {
        if (anisotropicModeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.AnisotropicMode = _anisotropicMode;

        switch (CurrentSettings.AnisotropicMode)
        {
            case 0:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                anisoFilteringModeText.text = "Disabled";

                anisoLevelObj.SetActive(false);
                Texture.SetGlobalAnisotropicFilteringLimits(-1, -1);
                break;
            case 1:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                anisoFilteringModeText.text = "Enabled";

                anisoLevelObj.SetActive(false);
                Texture.SetGlobalAnisotropicFilteringLimits(-1, -1);
                break;
            case 2:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                anisoFilteringModeText.text = "Forced";

                Texture.SetGlobalAnisotropicFilteringLimits(CurrentSettings.AnisotropicLevel, CurrentSettings.AnisotropicLevel);
                anisoFilteringLevelText.SetText("{0}x", CurrentSettings.AnisotropicLevel);
                anisoLevelObj.SetActive(true);
                break;
        }
    }

    public void UI_SetAnisotropicLevel(int _anisotropicLevel)
    {
        if (anisotropicLvlUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.AnisotropicLevel = _anisotropicLevel;

        Texture.SetGlobalAnisotropicFilteringLimits(CurrentSettings.AnisotropicLevel, CurrentSettings.AnisotropicLevel);
        anisoFilteringLevelText.SetText("{0}x", CurrentSettings.AnisotropicLevel);
    }

    public void UI_ResetToDefault()
    {
        if (isApplying)
            return;

        ApplySettings(DefaultSettingsConverted);       
    }

    //called when GraphicsMenu UIPanel is disabled or the menu is closed
    public void UI_SaveSettings()
    {
        if (!initiated || isApplying)
            return;

        SaveMenuVariables();
    }
    #endregion

    private void LoadMenuVariables()
    {
        if (saveFormat == SaveFormat.playerprefs)
        {
            if (PlayerPrefs.HasKey("Qualitylevel"))//to check if there are playerprefs saved.
            {
                MenuVariablesSimple_URP newMenuVars = new MenuVariablesSimple_URP();

                newMenuVars.Qualitylevel = PlayerPrefs.GetInt("Qualitylevel");
                newMenuVars.ResolutionX = PlayerPrefs.GetInt("ResolutionX");
                newMenuVars.ResolutionY = PlayerPrefs.GetInt("ResolutionY");
                newMenuVars.RenderScale = PlayerPrefs.GetFloat("RenderScale");
                newMenuVars.WindowedMode = PlayerPrefs.GetInt("WindowedMode");
                newMenuVars.VSync = PlayerPrefs.GetInt("VSync");
                newMenuVars.AntiAliaslevel = PlayerPrefs.GetInt("AntiAliaslevel");
                newMenuVars.TextureQuality = PlayerPrefs.GetInt("TextureQuality");
                newMenuVars.AnisotropicMode = PlayerPrefs.GetInt("AnisotropicMode");
                newMenuVars.AnisotropicLevel = PlayerPrefs.GetInt("AnisotropicLevel");

                ApplySettings(newMenuVars);

                newMenuVars = null;
            }
            else //no player prefs are saved.
            {
                //use the default values
                ApplySettings(DefaultSettingsConverted);
            }
        }
        else if (saveFormat == SaveFormat.iniFile)
        {
            if (File.Exists(saveFileDataPath))//to check if the file exists.
            {
                MenuVariablesSimple_URP newMenuVars = JsonUtility.FromJson<MenuVariablesSimple_URP>(File.ReadAllText(saveFileDataPath));

                ApplySettings(newMenuVars);

                newMenuVars = null;
            }
            else //no settings were saved.
            {
                //use the default values
                ApplySettings(DefaultSettingsConverted);
            }
        }
    }

    private void ApplySettings(MenuVariablesSimple_URP _varsLoaded)
    {
        isApplying = true;

        if (qualityLevelUsed == SettingsUsedState.used)
        {
            QualitySettings.SetQualityLevel(_varsLoaded.Qualitylevel);
            qualityLevelText.text = QualitySettings.names[_varsLoaded.Qualitylevel];
        }

        if (resolutionUsed == SettingsUsedState.used)
        {
            if (windowedModeUsed == SettingsUsedState.used)
            {
                switch (_varsLoaded.WindowedMode)
                {
                    case 0:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.ExclusiveFullScreen);
                        windowedModeText.text = "FullScreen";
                        break;
                    case 1:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.FullScreenWindow);
                        windowedModeText.text = "FullScreen Windowed";
                        break;
                    case 2:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.MaximizedWindow);
                        windowedModeText.text = "Maximized Windowed";
                        break;
                    case 3:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.Windowed);
                        windowedModeText.text = "Windowed";
                        break;
                }
            }
            else
            {
                Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, Screen.fullScreenMode);
            }

            resolutionText.SetText("{0}x{1}", _varsLoaded.ResolutionX, _varsLoaded.ResolutionY);

            //finding the applied resolution index NR
            for (int i = 0; i < availableResolutions.Count; i++)
            {
                if (availableResolutions[i].width == _varsLoaded.ResolutionX && availableResolutions[i].height == _varsLoaded.ResolutionY)
                {
                    currentResolutionIndex = i;
                    break;
                }
            }
        }
        else if (windowedModeUsed == SettingsUsedState.used)
        {
            switch (_varsLoaded.WindowedMode)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    windowedModeText.text = "FullScreen";
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    windowedModeText.text = "FullScreen Windowed";
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    windowedModeText.text = "Maximized Windowed";
                    break;
                case 3:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    windowedModeText.text = "Windowed";
                    break;
            }
        }

        if (antiAliasingUsed == SettingsUsedState.used)
        {
            UniversalRenderPipelineAsset URPAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
            URPAsset.msaaSampleCount = _varsLoaded.AntiAliaslevel;           
            URPAsset = null;

            if (_varsLoaded.AntiAliaslevel != 0)
                antiAliasingText.SetText("{0}x", _varsLoaded.AntiAliaslevel);
            else antiAliasingText.text = "Off";
        }

        if (renderScaleUsed == SettingsUsedState.used)
        {
            UniversalRenderPipelineAsset URPAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
            URPAsset.renderScale = _varsLoaded.RenderScale;
            URPAsset = null;

            renderScaleSlider.value = _varsLoaded.RenderScale * 10;
            renderScaleText.text = _varsLoaded.RenderScale.ToString();
        }

        if (vSyncUsed == SettingsUsedState.used)
        {
            QualitySettings.vSyncCount = _varsLoaded.VSync;

            switch (_varsLoaded.VSync)
            {
                case 0:
                    vsyncText.text = "Off";
                    break;
                case 1:
                    vsyncText.text = "Full";
                    break;
                case 2:
                    vsyncText.text = "Half";
                    break;
            }
        }

        if (textureQualityUsed == SettingsUsedState.used)
        {
            QualitySettings.globalTextureMipmapLimit = _varsLoaded.TextureQuality;

            switch (_varsLoaded.TextureQuality)
            {
                case 0:
                    textureQualityText.text = "Full";
                    break;
                case 1:
                    textureQualityText.text = "Half";
                    break;
                case 2:
                    textureQualityText.text = "Quarte";
                    break;
                case 3:
                    textureQualityText.text = "Eighth";
                    break;
            }
        }

        if (anisotropicModeUsed == SettingsUsedState.used)
        {
            switch (_varsLoaded.AnisotropicMode)
            {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    anisoFilteringModeText.text = "Disabled";

                    if (anisotropicLvlUsed == SettingsUsedState.used)
                    {
                        Texture.SetGlobalAnisotropicFilteringLimits(-1, -1);
                        anisoLevelObj.SetActive(false);
                    }
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    anisoFilteringModeText.text = "Enabled";

                    if (anisotropicLvlUsed == SettingsUsedState.used)
                    {
                        Texture.SetGlobalAnisotropicFilteringLimits(-1, -1);
                        anisoLevelObj.SetActive(false);
                    }
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    anisoFilteringModeText.text = "Forced";

                    if (anisotropicLvlUsed == SettingsUsedState.used)
                    {
                        Texture.SetGlobalAnisotropicFilteringLimits(_varsLoaded.AnisotropicLevel, _varsLoaded.AnisotropicLevel);
                        anisoFilteringLevelText.SetText("{0}x", _varsLoaded.AnisotropicLevel);
                        anisoLevelObj.SetActive(true);
                    }
                    break;
            }
        }
        else if (anisotropicLvlUsed == SettingsUsedState.used)
        {           
            Texture.SetGlobalAnisotropicFilteringLimits(_varsLoaded.AnisotropicLevel, _varsLoaded.AnisotropicLevel);
            anisoFilteringLevelText.SetText("{0}x", _varsLoaded.AnisotropicLevel);
        }

        CurrentSettings.Qualitylevel = _varsLoaded.Qualitylevel;
        CurrentSettings.ResolutionX = _varsLoaded.ResolutionX;
        CurrentSettings.ResolutionY = _varsLoaded.ResolutionY;
        CurrentSettings.RenderScale = _varsLoaded.RenderScale;
        CurrentSettings.WindowedMode = _varsLoaded.WindowedMode;
        CurrentSettings.VSync = _varsLoaded.VSync;
        CurrentSettings.AntiAliaslevel = _varsLoaded.AntiAliaslevel;
        CurrentSettings.TextureQuality = _varsLoaded.TextureQuality;
        CurrentSettings.AnisotropicMode = _varsLoaded.AnisotropicMode;
        CurrentSettings.AnisotropicLevel = _varsLoaded.AnisotropicLevel;

        isApplying = false;
    }

    private void SaveMenuVariables()
    {
        if (saveFormat == SaveFormat.playerprefs)
        {
            PlayerPrefs.SetInt("Qualitylevel", CurrentSettings.Qualitylevel);
            PlayerPrefs.SetInt("ResolutionX", CurrentSettings.ResolutionX);
            PlayerPrefs.SetInt("ResolutionY", CurrentSettings.ResolutionY);
            PlayerPrefs.SetFloat("RenderScale", CurrentSettings.RenderScale);
            PlayerPrefs.SetInt("WindowedMode", CurrentSettings.WindowedMode);
            PlayerPrefs.SetInt("VSync", CurrentSettings.VSync);
            PlayerPrefs.SetInt("AntiAliaslevel", CurrentSettings.AntiAliaslevel);
            PlayerPrefs.SetInt("TextureQuality", CurrentSettings.TextureQuality);
            PlayerPrefs.SetInt("AnisotropicMode", CurrentSettings.AnisotropicMode);
            PlayerPrefs.SetInt("AnisotropicLevel", CurrentSettings.AnisotropicLevel);
        }
        else if (saveFormat == SaveFormat.iniFile)
        {
            #region Setting the correct values for settings the are not used but will show on the ini file .
            MenuVariablesSimple_URP menuVarsToSave = new MenuVariablesSimple_URP();

            if (qualityLevelUsed == SettingsUsedState.used)
                menuVarsToSave.Qualitylevel = CurrentSettings.Qualitylevel;
            else menuVarsToSave.Qualitylevel = QualitySettings.GetQualityLevel();

            if (resolutionUsed == SettingsUsedState.used)
            {
                menuVarsToSave.ResolutionX = CurrentSettings.ResolutionX;
                menuVarsToSave.ResolutionY = CurrentSettings.ResolutionY;
            }
            else
            {
                menuVarsToSave.ResolutionX = Screen.currentResolution.width;
                menuVarsToSave.ResolutionY = Screen.currentResolution.height;
            }

            if (renderScaleUsed == SettingsUsedState.used)
                menuVarsToSave.RenderScale = CurrentSettings.RenderScale;
            else
            {
                UniversalRenderPipelineAsset URPAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
                menuVarsToSave.RenderScale = URPAsset.renderScale;
                URPAsset = null;
            }

            if (windowedModeUsed == SettingsUsedState.used)
                menuVarsToSave.WindowedMode = CurrentSettings.WindowedMode;
            else
            {
                switch (Screen.fullScreenMode)
                {
                    case FullScreenMode.ExclusiveFullScreen:
                        menuVarsToSave.WindowedMode = 0;
                        break;
                    case FullScreenMode.FullScreenWindow:
                        menuVarsToSave.WindowedMode = 1;
                        break;
                    case FullScreenMode.MaximizedWindow:
                        menuVarsToSave.WindowedMode = 2;
                        break;
                    case FullScreenMode.Windowed:
                        menuVarsToSave.WindowedMode = 3;
                        break;
                }
            }

            if (vSyncUsed == SettingsUsedState.used)
                menuVarsToSave.VSync = CurrentSettings.VSync;
            else menuVarsToSave.VSync = QualitySettings.vSyncCount;

            if (antiAliasingUsed == SettingsUsedState.used)
                menuVarsToSave.AntiAliaslevel = CurrentSettings.AntiAliaslevel;
            else
            {
                UniversalRenderPipelineAsset URPAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
                menuVarsToSave.AntiAliaslevel = URPAsset.msaaSampleCount;
                URPAsset = null;
            }

            if (textureQualityUsed == SettingsUsedState.used)
                menuVarsToSave.TextureQuality = CurrentSettings.TextureQuality;
            else menuVarsToSave.TextureQuality = QualitySettings.globalTextureMipmapLimit;

            if (anisotropicModeUsed == SettingsUsedState.used)
                menuVarsToSave.AnisotropicMode = CurrentSettings.AnisotropicMode;
            else
            {
                switch (QualitySettings.anisotropicFiltering)
                {
                    case AnisotropicFiltering.Disable:
                        menuVarsToSave.AnisotropicMode = 0;
                        break;
                    case AnisotropicFiltering.Enable:
                        menuVarsToSave.AnisotropicMode = 1;
                        break;
                    case AnisotropicFiltering.ForceEnable:
                        menuVarsToSave.AnisotropicMode = 2;
                        break;
                }
            }

            if (anisotropicLvlUsed == SettingsUsedState.used)
                menuVarsToSave.AnisotropicLevel = CurrentSettings.AnisotropicLevel;
            else menuVarsToSave.AnisotropicLevel = -1;//default used

            menuVarsToSave.Warning = DefaultSettingsConverted.Warning;
            #endregion

            File.WriteAllText(saveFileDataPath, JsonUtility.ToJson(menuVarsToSave, true));
        }
    }
}

//custom classes

    //easier to read and adjust in the inspector
    [System.Serializable]
    public class MenuVariables_URP
    {
        public int Qualitylevel = 1;

        [Header("Setting one or both to Zero, will use the monitors/windows resolution.")]
        public Vector2Int Resolution = new Vector2Int(0, 0);
        [Range(0.1f, 2f)]
        public float RenderScale = 1f;
        public FullScreenMode WindowedMode = FullScreenMode.MaximizedWindow;
        public VSyncEnum VSync = VSyncEnum.off;
        public AntiAliasLevelEnum AntiAliaslevel = 0;
        public TextureQualityEnum TextureQuality = TextureQualityEnum.FullRes;
        public AnisotropicFiltering AnisotropicMode = AnisotropicFiltering.Enable;

        [Header("Used when Anisotropic Mode = forced Enabled")]
        public AnisotropicLevelEnum AnisotropicLevel = AnisotropicLevelEnum.x4;

        [Header("A Warning for users changing the ini file.")]
        public string WarningMessage = "Edit this file at your own risk!";
    }

    //easier to use in script
    [System.Serializable]
    public class MenuVariablesSimple_URP
    {
        public int Qualitylevel;
        public int ResolutionX, ResolutionY;
        public float RenderScale;
        public int WindowedMode;
        public int VSync;
        public int AntiAliaslevel;
        public int TextureQuality;
        public int AnisotropicMode;
        public int AnisotropicLevel;

        public string Warning;
    }

    public enum SettingsUsedState { used, notUsed };
    public enum VSyncEnum { off, full, half };
    public enum AntiAliasLevelEnum { off, x2, x4, x8 };
    public enum TextureQualityEnum { FullRes, HalfRes, QuarterRes, EighthRes };
    public enum AnisotropicLevelEnum { x2, x4, x8, x16 };
}