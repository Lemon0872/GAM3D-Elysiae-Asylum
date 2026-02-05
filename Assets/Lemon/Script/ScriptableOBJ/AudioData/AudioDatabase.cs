using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class AudioDatabase : ScriptableObject
{
    public List<SoundData> sfxList;
    public List<MusicData> musicList;

    Dictionary<string, SoundData> sfxDict;
    Dictionary<string, MusicData> musicDict;

    public void Init()
    {
        sfxDict = new Dictionary<string, SoundData>();
        musicDict = new Dictionary<string, MusicData>();

        foreach (var sfx in sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.id))
                sfxDict.Add(sfx.id, sfx);
        }

        foreach (var music in musicList)
        {
            if (!musicDict.ContainsKey(music.id))
                musicDict.Add(music.id, music);
        }
    }

    public SoundData GetSFX(string id)
        => sfxDict.TryGetValue(id, out var s) ? s : null;

    public MusicData GetMusic(string id)
        => musicDict.TryGetValue(id, out var m) ? m : null;
}
