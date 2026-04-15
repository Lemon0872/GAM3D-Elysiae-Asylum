using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool
{
    List<AudioSource> pool = new();
    Transform root;

    public AudioSourcePool(Transform root, int init = 10)
    {
        this.root = root;
        for (int i = 0; i < init; i++)
            pool.Add(Create());
    }

    AudioSource Create()
    {
        var go = new GameObject("PooledAudio");
        go.transform.parent = root;
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        return src;
    }

    public AudioSource Get()
    {
        foreach (var src in pool)
            if (!src.isPlaying)
                return src;

        var extra = Create();
        pool.Add(extra);
        return extra;
    }
}
