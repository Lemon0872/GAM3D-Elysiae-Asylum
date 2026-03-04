using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] float fadeTime = 0.3f;
    Coroutine masterFadeRoutine;
    Coroutine musicFadeRoutine;
    Coroutine sfxFadeRoutine;

    [Header("Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Toggles")]
    [SerializeField] Toggle masterToggle;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    [Header("UI")]
    [SerializeField] GameObject panelSetting;
    [SerializeField] GameObject panelDarkUI;

    [Header("Animation")]
    [SerializeField] Animator animator;
    public bool isMouseLock;

    const string MASTER_VOL = "Master";
    const string MUSIC_VOL  = "Music";
    const string SFX_VOL    = "SFX";

    const string MASTER_ON = "Master";
    const string MUSIC_ON  = "Music";
    const string SFX_ON    = "SFX";

    void Start()
    {
        Load();
        ApplyAll();
        panelSetting.SetActive(false);
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panelSetting.activeSelf)
            {
                animator.SetBool("IsClose",false);
                panelSetting.SetActive(true);
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            else
            {
                animator.SetBool("IsClose",true);
                if (isMouseLock)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                
                Time.timeScale = 1f;
            }
        }
        panelDarkUI.SetActive(panelSetting.activeSelf);
    }
    
    // ===== Slider Callbacks =====
    public void OnMasterSliderChanged(float value)
    {
        PlayerPrefs.SetFloat(MASTER_VOL, value);

        if (!masterToggle.isOn) return;

        mixer.SetFloat(MASTER_VOL, SliderToDb(masterSlider));
    }


    public void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat(MUSIC_VOL, value);

        if (!musicToggle.isOn) return;

        mixer.SetFloat(MUSIC_VOL, SliderToDb(musicSlider));
    }


    public void OnSFXSliderChanged(float value)
    {
        PlayerPrefs.SetFloat(SFX_VOL, value);

        if (!sfxToggle.isOn) return;

        mixer.SetFloat(SFX_VOL, SliderToDb(sfxSlider));
    }


    // ===== Toggle Callbacks =====
    public void OnMasterToggleChanged(bool on)
    {
        if (masterFadeRoutine != null)
            StopCoroutine(masterFadeRoutine);

        PlayerPrefs.SetInt(MASTER_ON, on ? 1 : 0);

        if (on)
        {
            float targetDb = SliderToDb(masterSlider);
            masterFadeRoutine = StartCoroutine(
                FadeMixer(MASTER_VOL, targetDb, fadeTime)
            );
        }
        else
        {
            masterFadeRoutine = StartCoroutine(
                FadeMixer(MASTER_VOL, -80f, fadeTime)
            );
        }
    }


    public void OnMusicToggleChanged(bool on)
    {
        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        PlayerPrefs.SetInt(MUSIC_ON, on ? 1 : 0);

        if (on)
        {
            float targetDb = SliderToDb(musicSlider);
            musicFadeRoutine = StartCoroutine(
                FadeMixer(MUSIC_VOL, targetDb, fadeTime)
            );
        }
        else
        {
            musicFadeRoutine = StartCoroutine(
                FadeMixer(MUSIC_VOL, -80f, fadeTime)
            );
        }
    }


    public void OnSFXToggleChanged(bool on)
    {
        if (sfxFadeRoutine != null)
            StopCoroutine(sfxFadeRoutine);

        PlayerPrefs.SetInt(SFX_ON, on ? 1 : 0);

        if (on)
        {
            float targetDb = SliderToDb(sfxSlider);
            sfxFadeRoutine = StartCoroutine(
                FadeMixer(SFX_VOL, targetDb, fadeTime)
            );
        }
        else
        {
            sfxFadeRoutine = StartCoroutine(
                FadeMixer(SFX_VOL, -80f, fadeTime)
            );
        }
    }


    float SliderToDb(Slider slider)
    {
        float v = Mathf.Max(slider.value, 0.0001f);
        return Mathf.Log10(v) * 20f;
    }

    IEnumerator FadeMixer(string param, float targetDb, float duration)
    {
        mixer.GetFloat(param, out float currentDb);

        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;
            float value = Mathf.Lerp(currentDb, targetDb, t);
            mixer.SetFloat(param, value);
            yield return null;
        }

        mixer.SetFloat(param, targetDb);
    }

    void Load()
    {
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOL, 1f);
        musicSlider.value  = PlayerPrefs.GetFloat(MUSIC_VOL, 1f);
        sfxSlider.value    = PlayerPrefs.GetFloat(SFX_VOL, 1f);

        masterToggle.isOn = PlayerPrefs.GetInt(MASTER_ON, 1) == 1;
        musicToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(MUSIC_ON, 1) == 1);
        sfxToggle.isOn    = PlayerPrefs.GetInt(SFX_ON, 1) == 1;
    }

    void ApplyAll()
    {
        if (masterToggle.isOn) mixer.SetFloat(MASTER_VOL, SliderToDb(masterSlider));
        else mixer.SetFloat(MASTER_VOL, -80f);
        if (musicToggle.isOn) mixer.SetFloat(MUSIC_VOL, SliderToDb(musicSlider));
        else mixer.SetFloat(MUSIC_VOL, -80f);
        if (sfxToggle.isOn) mixer.SetFloat(SFX_VOL, SliderToDb(sfxSlider));
        else mixer.SetFloat(SFX_VOL, -80f);
    }
}
