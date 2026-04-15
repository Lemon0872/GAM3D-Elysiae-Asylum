using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettingsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image brightnessOverlay;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Brightness Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float maxBrightnessAlpha = 0.7f;

    private const string BRIGHTNESS_KEY = "Brightness";
    private const string QUALITY_KEY = "Quality";
    private const string FULLSCREEN_KEY = "Fullscreen";

    // ==============================
    // Initialization
    // ==============================

    private void Start()
    {
        InitializeQualityDropdown();
        LoadSettings();
        BindUI();
    }

    private void BindUI()
    {
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    // ==============================
    // Brightness
    // ==============================

    public void SetBrightness(float value)
    {
        value = Mathf.Clamp01(value);

        float darkness = 1f - value;

        brightnessOverlay.color = new Color(0f, 0f, 0f, darkness);
    }



    // ==============================
    // Quality
    // ==============================

    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(QUALITY_KEY, qualityIndex);
    }

    // ==============================
    // Fullscreen
    // ==============================

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
    }

    // ==============================
    // Load Saved Settings
    // ==============================

    private void LoadSettings()
    {
        // Brightness
        float savedBrightness = PlayerPrefs.GetFloat(BRIGHTNESS_KEY, 1f);
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);

        // Quality
        int savedQuality = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQuality;
        QualitySettings.SetQualityLevel(savedQuality);

        // Fullscreen
        bool savedFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) == 1;
        fullscreenToggle.isOn = savedFullscreen;
        Screen.fullScreen = savedFullscreen;
    }
}
