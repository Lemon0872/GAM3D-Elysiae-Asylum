using UnityEngine;

public class FinalPrefabEffects : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private SoundData destroySound;   // kéo file .asset SoundData vào đây

    [Header("Particle Settings")]
    [SerializeField] private GameObject particlePrefab; // prefab particle effect

    private void OnDestroy()
    {
        // Phát âm thanh bằng cách tạo AudioSource tạm thời
        if (destroySound != null && destroySound.clip != null)
        {
            AudioSource tempSource = new GameObject("TempAudio").AddComponent<AudioSource>();
            tempSource.transform.position = transform.position;

            tempSource.clip = destroySound.clip;
            tempSource.outputAudioMixerGroup = destroySound.mixer;
            tempSource.volume = destroySound.volume;
            tempSource.loop = destroySound.loop;
            tempSource.pitch = Random.Range(destroySound.pitchRange.x, destroySound.pitchRange.y);

            if (destroySound.is3D)
            {
                tempSource.spatialBlend = 1f;
                tempSource.minDistance = destroySound.minDistance;
                tempSource.maxDistance = destroySound.maxDistance;
                tempSource.rolloffMode = destroySound.rolloff;
            }
            else
            {
                tempSource.spatialBlend = 0f;
            }

            tempSource.Play();
            Destroy(tempSource.gameObject, destroySound.clip.length / tempSource.pitch);
        }

        // Spawn particle effect
        if (particlePrefab != null)
        {
            GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, 3f); // tự hủy sau 3 giây
        }
    }
}
