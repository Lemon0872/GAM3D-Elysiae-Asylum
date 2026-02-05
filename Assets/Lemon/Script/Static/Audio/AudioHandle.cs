using UnityEngine;

public class AudioHandle
{
    AudioSource source;

    public AudioHandle(AudioSource src)
    {
        source = src;
    }

    public void Stop()
    {
        if (source != null)
            source.Stop();
    }

    public bool IsPlaying => source != null && source.isPlaying;
}
