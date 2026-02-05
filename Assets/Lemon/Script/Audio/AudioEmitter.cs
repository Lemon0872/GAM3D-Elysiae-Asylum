using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    public void Play(string sfxId)
    {
        AudioManager.PlaySFXAt(sfxId, transform);
    }
}
