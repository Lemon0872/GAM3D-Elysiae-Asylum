using UnityEngine;

public class SFXChannel
{
    AudioDatabase db;
    AudioSourcePool pool;

    public SFXChannel(AudioDatabase db, AudioSourcePool pool)
    {
        this.db = db;
        this.pool = pool;
    }

    public void PlayAt(string id, Transform emitter)
    {
        var data = db.GetSFX(id);
        if (data == null) return;

        var src = pool.Get();
        src.clip = data.clip;
        src.volume = data.volume;
        src.pitch = Random.Range(data.pitchRange.x, data.pitchRange.y);
        src.loop = data.loop;
        src.outputAudioMixerGroup = data.mixer;

        src.transform.position = emitter.position;
        src.spatialBlend = data.is3D ? 1f : 0f;
        src.minDistance = data.minDistance;
        src.maxDistance = data.maxDistance;
        src.rolloffMode = data.rolloff;

        src.Play();
    }
}
