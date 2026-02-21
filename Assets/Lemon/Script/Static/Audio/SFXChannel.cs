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

    public void PlayAt(string id, Vector3 emitter)
    {
        var data = db.GetSFX(id);
        if (data == null || data.clip == null)
            return;

        var src = pool.Get();

        src.clip = data.clip;
        src.volume = data.volume;
        src.pitch = Random.Range(data.pitchRange.x, data.pitchRange.y);
        src.loop = data.loop;
        src.outputAudioMixerGroup = data.mixer;

        if (data.is3D)
        {
            src.spatialBlend = 1f;

            src.transform.position = emitter;
            src.minDistance = data.minDistance;
            src.maxDistance = data.maxDistance;
            src.rolloffMode = data.rolloff;
        }
        else
        {
            src.spatialBlend = 0f;
            src.transform.localPosition = Vector3.zero;
        }

        src.Play();
    }
}
