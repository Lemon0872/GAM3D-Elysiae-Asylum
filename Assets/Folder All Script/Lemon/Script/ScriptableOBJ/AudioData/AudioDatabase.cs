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
            string key = sfx.name;
            if (!sfxDict.ContainsKey(key))
                sfxDict.Add(key, sfx);
        }

        foreach (var music in musicList)
        {
            string key = music.name;
            if (!musicDict.ContainsKey(key))
                musicDict.Add(key, music);
        }
    }

    public SoundData GetSFX(string id)
        => sfxDict.TryGetValue(id, out var s) ? s : null;

    public MusicData GetMusic(string id)
        => musicDict.TryGetValue(id, out var m) ? m : null;
}
