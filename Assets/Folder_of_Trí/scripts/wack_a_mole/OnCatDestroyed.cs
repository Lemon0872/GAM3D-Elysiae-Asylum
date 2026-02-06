using UnityEngine;

public class FinalPrefabEffects : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private SoundData destroySound;   // kéo file .asset SoundData vào đây
    [SerializeField] private AudioSource audioSource;  // gắn AudioSource vào prefab

    [Header("Particle Settings")]
    [SerializeField] private GameObject particlePrefab; // prefab particle effect

    private void OnDestroy()
    {
        // Phát âm thanh
        if (destroySound != null && audioSource != null)
        {
            audioSource.clip = destroySound.clip;
            audioSource.outputAudioMixerGroup = destroySound.mixer;
            audioSource.volume = destroySound.volume;
            audioSource.loop = destroySound.loop;
            audioSource.pitch = Random.Range(destroySound.pitchRange.x, destroySound.pitchRange.y);

            if (destroySound.is3D)
            {
                audioSource.spatialBlend = 1f;
                audioSource.minDistance = destroySound.minDistance;
                audioSource.maxDistance = destroySound.maxDistance;
                audioSource.rolloffMode = destroySound.rolloff;
            }
            else
            {
                audioSource.spatialBlend = 0f;
            }

            audioSource.Play();
        }

        // Spawn particle effect
        if (particlePrefab != null)
        {
            GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, 3f); // tự hủy sau 3 giây để tránh rác
        }
    }
}
