using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/SFX Data")]
public class SoundData : ScriptableObject
{
    [Header("ID")]
    public string id;

    [Header("Clip")]
    public AudioClip clip;
    public AudioMixerGroup mixer;

    [Header("General")]
    public float volume = 1f;
    public Vector2 pitchRange = Vector2.one;
    public bool loop;

    [Header("3D Settings")]
    public bool is3D = true;
    public float minDistance = 1f;
    public float maxDistance = 20f;
    public AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;
}
