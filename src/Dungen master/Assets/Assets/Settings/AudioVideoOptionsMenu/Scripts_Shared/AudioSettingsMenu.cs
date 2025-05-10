using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings.AudioVideoOptionsMenu
{
    public class AudioSettingsMenu : MonoBehaviour
{
    [Header("The Audio Mixer Controller you are using")]
    [SerializeField]
    private AudioMixer masterMixer;//mixer prefab

    [Header("Select wich settings you want to use. Settings set to[UnUsed] can be removed from/disabled in the menu UI")]
    [Space(10)]
    [SerializeField]
    [Tooltip("Changing SpeakerMode Will Reset/Stop all current playing audio !!")]
    private SettingsUsedState speakerModeUsed;
    [SerializeField]
    private SettingsUsedState musicVolumeUsed, fxVolumeUsed, mainVolumeUsed, muteUsed;

    [Header("UI elements references")]
    [Space(10)]
    [SerializeField]
    private TMP_Text mainVolumeText;
    [SerializeField]
    private TMP_Text fxsVolumeText, musicVolumeText, speakerModeText;
    [SerializeField]
    private Slider mainVolumeSlider, fxsVolumeSlider, musicVolumeSlider;
    [SerializeField]
    private Toggle muteToggle;

    //default values
    private bool isMuted = false;
    private float currentMainVolume = 1, currentFXsVolume = 1, currentMusicVolume = 1;
    private int currentSpeakerMode = 1;//stereo

    private bool initiated = false, isApplying;

    // Use this for initialization
    void Start()
    {
        LoadMenuVariables();//needs to be in Start, masterMixer not ready yet in Awake

        initiated = true;
    }

    public void UI_ToggleMute()
    {
        if (muteUsed == SettingsUsedState.notUsed || isApplying)
            return;

        isMuted = !isMuted;

        if (isMuted)
            masterMixer.SetFloat("mainVolume", -80);
        else masterMixer.SetFloat("mainVolume", Mathf.Log10(currentMainVolume) * 20);
    }

    public void UI_SetMainVolume(Slider _slider)
    {
        if (mainVolumeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        currentMainVolume = _slider.value;

        if (!isMuted)
            masterMixer.SetFloat("mainVolume", Mathf.Log10(currentMainVolume) * 20);

        mainVolumeText.SetText("{0:0}%", _slider.value * 100);
    }

    public void UI_SetFXsVolume(Slider _slider)
    {
        if (fxVolumeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        currentFXsVolume = _slider.value;
        masterMixer.SetFloat("fxVolume", Mathf.Log10(currentFXsVolume) * 20);

        fxsVolumeText.SetText("{0:0}%", _slider.value * 100);
    }

    public void UI_SetMusicVolume(Slider _slider)
    {
        if (musicVolumeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        currentMusicVolume = _slider.value;
        masterMixer.SetFloat("musicVolume", Mathf.Log10(currentMusicVolume) * 20);

        musicVolumeText.SetText("{0:0}%", _slider.value * 100);
    }

    //Changing SpeakerMode Will Reset/Stop all current playing audio !!
    public void UI_SetSpeakerMode(int _speakerMode)
    {
        if (speakerModeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        currentSpeakerMode = _speakerMode;

        var config = AudioSettings.GetConfiguration();

        var wantedMode = config.speakerMode;
        var wantedModeText = "";

        switch (currentSpeakerMode)
        {
            case 0:
                wantedMode = AudioSpeakerMode.Mono;
                wantedModeText = "Mono";
                break;
            case 1:
                wantedMode = AudioSpeakerMode.Stereo;
                wantedModeText = "Stereo";
                break;
            case 2:
                wantedMode = AudioSpeakerMode.Quad;
                wantedModeText = "4.4";
                break;
            case 3:
                wantedMode = AudioSpeakerMode.Surround;
                wantedModeText = "Surround";
                break;
            case 4:
                wantedMode = AudioSpeakerMode.Mode5point1;
                wantedModeText = "6.5.1";
                break;
            case 5:
                wantedMode = AudioSpeakerMode.Mode7point1;
                wantedModeText = "8.7.1";
                break;
            case 6:
                wantedMode = AudioSpeakerMode.Prologic;
                wantedModeText = "Prologic";
                break;
        }

        if (wantedMode != config.speakerMode)
        {
            config.speakerMode = wantedMode;
            speakerModeText.text = wantedModeText;

            AudioSettings.OnAudioConfigurationChanged += OnSpeakerModeChanged;
            AudioSettings.Reset(config);
        }
    }

    //called when AudioMenu UIPanel is disabled or the menu is closed
    public void UI_SaveSettings()
    {
        if (!initiated)
            return;

        SaveMenuVariables();
    }

    private void LoadMenuVariables()
    {
        if (PlayerPrefs.HasKey("muted"))
        {
            if (mainVolumeUsed == SettingsUsedState.used)
                currentMainVolume = PlayerPrefs.GetFloat("mainVolume");//triggers the UI_SetMainVolume function

            if (fxVolumeUsed == SettingsUsedState.used)
                currentFXsVolume = PlayerPrefs.GetFloat("fxVolume");//triggers the UI_SetFXsVolume function

            if (musicVolumeUsed == SettingsUsedState.used)
                currentMusicVolume = PlayerPrefs.GetFloat("musicVolume");//triggers the UI_SetMusicVolume function

            if (speakerModeUsed == SettingsUsedState.used)
                currentSpeakerMode = PlayerPrefs.GetInt("speakerMode");

            if (muteUsed == SettingsUsedState.used)
                isMuted = PlayerPrefs.GetInt("muted") == 1 ? true : false;
        }

        ApplySettings();
    }

    private void ApplySettings()
    {
        isApplying = true;

        if (speakerModeUsed == SettingsUsedState.used)//must be first
        {
            var config = AudioSettings.GetConfiguration();

            var wantedMode = config.speakerMode;
            var wantedModeText = "";

            switch (currentSpeakerMode)
            {
                case 0:
                    wantedMode = AudioSpeakerMode.Mono;
                    wantedModeText = "Mono";
                    break;
                case 1:
                    wantedMode = AudioSpeakerMode.Stereo;
                    wantedModeText = "Stereo";
                    break;
                case 2:
                    wantedMode = AudioSpeakerMode.Quad;
                    wantedModeText = "4.4";
                    break;
                case 3:
                    wantedMode = AudioSpeakerMode.Surround;
                    wantedModeText = "Surround";
                    break;
                case 4:
                    wantedMode = AudioSpeakerMode.Mode5point1;
                    wantedModeText = "6.5.1";
                    break;
                case 5:
                    wantedMode = AudioSpeakerMode.Mode7point1;
                    wantedModeText = "8.7.1";
                    break;
                case 6:
                    wantedMode = AudioSpeakerMode.Prologic;
                    wantedModeText = "Prologic";
                    break;
            }

            if (wantedMode != config.speakerMode)
            {
                config.speakerMode = wantedMode;

                AudioSettings.Reset(config);
            }

            speakerModeText.text = wantedModeText;
        }

        if (mainVolumeUsed == SettingsUsedState.used)
        {
            masterMixer.SetFloat("mainVolume", Mathf.Log10(currentMainVolume) * 20);

            mainVolumeSlider.value = currentMainVolume;
            mainVolumeText.SetText("{0:0}%", mainVolumeSlider.value * 100);
        }

        if (fxVolumeUsed == SettingsUsedState.used)
        {
            masterMixer.SetFloat("fxVolume", Mathf.Log10(currentFXsVolume) * 20);

            fxsVolumeSlider.value = currentFXsVolume;
            fxsVolumeText.SetText("{0:0}%", fxsVolumeSlider.value * 100);
        }

        if (musicVolumeUsed == SettingsUsedState.used)
        {
            masterMixer.SetFloat("musicVolume", Mathf.Log10(currentMusicVolume) * 20);

            musicVolumeSlider.value = currentMusicVolume;
            musicVolumeText.SetText("{0:0}%", musicVolumeSlider.value * 100);
        }

        if (muteUsed == SettingsUsedState.used)//must be last
        {
            if (isMuted)
                masterMixer.SetFloat("mainVolume", -80);
            else masterMixer.SetFloat("mainVolume", Mathf.Log10(currentMainVolume) * 20);

            muteToggle.isOn = isMuted;
        }

        isApplying = false;
    }

    //triggered when Speakermode has changed, need to reApply the settings (changing speakermode resets all audio)
    private void OnSpeakerModeChanged(bool _wasChanged)
    {
        if (mainVolumeUsed == SettingsUsedState.used)
        {
            masterMixer.SetFloat("mainVolume", Mathf.Log10(currentMainVolume) * 20);       
        }

        if (fxVolumeUsed == SettingsUsedState.used)
        {
            masterMixer.SetFloat("fxVolume", Mathf.Log10(currentFXsVolume) * 20);
        }

        if (musicVolumeUsed == SettingsUsedState.used)
        {
            masterMixer.SetFloat("musicVolume", Mathf.Log10(currentMusicVolume) * 20);
        }

        if (muteUsed == SettingsUsedState.used)//must be last
        {
            if (isMuted)
                masterMixer.SetFloat("mainVolume", -80);
            else masterMixer.SetFloat("mainVolume", Mathf.Log10(currentMainVolume) * 20);
        }

        AudioSettings.OnAudioConfigurationChanged -= OnSpeakerModeChanged;
    }

    private void SaveMenuVariables()
    {
        PlayerPrefs.SetInt("muted", isMuted == true ? 1 : 0);
        PlayerPrefs.SetFloat("mainVolume", currentMainVolume);
        PlayerPrefs.SetFloat("fxVolume", currentFXsVolume);
        PlayerPrefs.SetFloat("musicVolume", currentMusicVolume);
        PlayerPrefs.SetInt("speakerMode", currentSpeakerMode);
    }
}
}

