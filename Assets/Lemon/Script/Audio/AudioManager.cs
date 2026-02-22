using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioDatabase database;

    SFXChannel sfx;
    MusicChannel music;
    AudioSourcePool pool;

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

    public static void PlaySFXAt(string id, Transform emitter)
        => Instance.sfx.PlayAt(id, emitter);

    public static void PlayMusic(string id)
        => Instance.music.Play(id);
}
