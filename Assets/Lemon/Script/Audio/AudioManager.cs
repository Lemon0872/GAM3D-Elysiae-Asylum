using UnityEngine;
using UnityEngine.Audio;

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
