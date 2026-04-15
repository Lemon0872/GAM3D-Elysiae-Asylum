using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/SFX Data")]
public class SoundData : ScriptableObject
{
    public AudioClip clip;
    public AudioMixerGroup mixer;

    [Header("Volume & Pitch")]
    public float volume = 1f;
    public Vector2 pitchRange = Vector2.one;

    [Header("3D Settings")]
    [Tooltip("Âm thanh 3D")]
    public bool is3D = false;
    public float minDistance = 1f;
    public float maxDistance = 20f;
    public AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;

    [Header("Loop")]
    public bool loop = false;
}
