using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //AudioManager.PlayMusic("[tên scriptable music]");
    //AudioManager.PlaySFXAt("[tên scriptable SFX]", [vector3 vị trí phát]);
    public static AudioManager Instance;
    [SerializeField] AudioDatabase database;
    SFXChannel sfx;
    MusicChannel music;
    AudioSourcePool pool;

    [Header("Snapshots")]
    public AudioMixerSnapshot normalSnapshot;
    public AudioMixerSnapshot uiFocusSnapshot;
    public HashSet<Button> installedButtons = new HashSet<Button>();

    [Header("Settings")]
    public float transitionTime = 0.5f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        database.Init();

        pool = new AudioSourcePool(transform);
        sfx = new SFXChannel(database, pool);
        music = new MusicChannel(database, transform, this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name=="Menu") PlayMusic("MenuTheme");
        InstallAllButtons();
    }

    void InstallAllButtons()
    {
        var buttons = FindObjectsOfType<Button>(true);

        foreach (var btn in buttons)
        {
            if (installedButtons.Contains(btn))
                continue;

            btn.onClick.AddListener(() =>
            {
                PlayClickSound();
            });

            installedButtons.Add(btn);
        }
    }

    void PlayClickSound()
    {
        PlaySFXAt("UI[Button]Click",transform.position);
    }

    public static void PlaySFXAt(string id, Vector3 emitter)
        => Instance.sfx.PlayAt(id, emitter);

    public static void PlayMusic(string id)
        => Instance.music.Play(id);
    public void EnterUIFocus()
    {
        uiFocusSnapshot.TransitionTo(transitionTime);
    }

    public void ExitUIFocus()
    {
        normalSnapshot.TransitionTo(transitionTime);
    }
}
