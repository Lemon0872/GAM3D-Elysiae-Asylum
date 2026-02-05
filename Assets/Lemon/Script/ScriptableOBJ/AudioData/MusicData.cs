using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Music Data")]
public class MusicData : ScriptableObject
{
    public string id;

    public AudioClip clip;
    public AudioMixerGroup mixer;

    [Header("Volume")]
    public float volume = 1f;

    [Header("Fade")]
    public float fadeIn = 1f;
    public float fadeOut = 1f;
}
