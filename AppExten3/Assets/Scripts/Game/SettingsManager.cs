using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown shadowsDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    private Resolution[] resolutions;
    private string settingsPath;
    private List<Resolution> uniqueResList = new List<Resolution>();

    void Awake()
    {
        settingsPath = Application.persistentDataPath + "/settings.json";
        LoadSettings();
    }

    void Start()
    {
        // Volume
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Shadows
        shadowsDropdown.ClearOptions();
        List<string> shadowOptions = new List<string> { "Off", "Hard Only", "All" };
        shadowsDropdown.AddOptions(shadowOptions);
        shadowsDropdown.onValueChanged.AddListener(SetShadows);

        // Resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        HashSet<string> added = new HashSet<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resString = resolutions[i].width + " x " + resolutions[i].height;
            if (added.Add(resString))
            {
                options.Add(resString);
                uniqueResList.Add(resolutions[i]);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = uniqueResList.Count - 1;
                }
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Fullscreen
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // VSync
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vsyncToggle.onValueChanged.AddListener(SetVSync);

        LoadSettings();

    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < uniqueResList.Count)
        {
            Resolution res = uniqueResList[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVSync(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;
    }

    public void SetShadows(int index)
    {
        switch (index)
        {
            case 0: QualitySettings.shadows = ShadowQuality.Disable; break;
            case 1: QualitySettings.shadows = ShadowQuality.HardOnly; break;
            case 2: QualitySettings.shadows = ShadowQuality.All; break;
        }
    }
    public void SaveSettings()
    {
        Resolution currentRes = Screen.currentResolution;

        SettingsData data = new SettingsData()
        {
            volume = volumeSlider.value,
            shadowIndex = shadowsDropdown.value,
            resolutionIndex = resolutionDropdown.value,
            screenWidth = currentRes.width,
            screenHeight = currentRes.height,
            isFullscreen = fullscreenToggle.isOn,
            vsyncEnabled = vsyncToggle.isOn
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(settingsPath, json);
        Debug.Log("Settings saved to " + settingsPath);
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            SettingsData data = JsonUtility.FromJson<SettingsData>(json);

            volumeSlider.value = data.volume;
            shadowsDropdown.value = data.shadowIndex;
            resolutionDropdown.value = data.resolutionIndex;
            fullscreenToggle.isOn = data.isFullscreen;
            vsyncToggle.isOn = data.vsyncEnabled;

            ApplyAllSettings(data);
        }
        else
        {
            Debug.Log("No settings file found, using defaults.");
        }
    }

    void ApplyAllSettings(SettingsData data)
    {
        SetVolume(data.volume);
        SetQuality(data.shadowIndex);
        SetResolution(data.resolutionIndex);
        SetFullscreen(data.isFullscreen);
        SetVSync(data.vsyncEnabled);
    }

    [System.Serializable]
    public class SettingsData
    {
        public float volume;
        public int shadowIndex;
        public int resolutionIndex;
        public int screenWidth;
        public int screenHeight;
        public bool isFullscreen;
        public bool vsyncEnabled;
    }
}
