using UnityEngine;
using System.Collections;

public class MusicChannel
{
    AudioSource source;
    AudioDatabase db;
    MonoBehaviour runner;

    public MusicChannel(AudioDatabase db, Transform root, MonoBehaviour runner)
    {
        this.db = db;
        this.runner = runner;

        source = new GameObject("MusicSource").AddComponent<AudioSource>();
        source.transform.parent = root;
        source.loop = true;
    }

    public void Play(string id)
    {
        var data = db.GetMusic(id);
        if (data == null) return;

        runner.StartCoroutine(Transition(data));
    }

    IEnumerator Transition(MusicData data)
    {
        if (source.isPlaying)
            yield return FadeOut(data.fadeOut);

        source.clip = data.clip;
        source.volume = 0f;
        source.outputAudioMixerGroup = data.mixer;
        source.Play();

        yield return FadeIn(data.fadeIn, data.volume);
    }

    IEnumerator FadeIn(float time, float target)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(0, target, t / time);
            yield return null;
        }
        source.volume = target;
    }

    IEnumerator FadeOut(float time)
    {
        float start = source.volume;
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(start, 0, t / time);
            yield return null;
        }
        source.Stop();
    }
}
