using UnityEngine;
using System.Collections.Generic;

public class MoleSpawner : MonoBehaviour
{
    public GameObject molePrefab;
    public Vector3 spawnCenter;
    public Vector3 spawnRange;

    public float spawnInterval = 2f;
    private float timer;

    [Header("Spawn Limit Settings")]
    public int maxMoles = 10; // số mole tối đa trong phạm vi
    private List<GameObject> activeMoles = new List<GameObject>();

    [Header("Special Spawn Settings")]
    public int specialSpawnCount = 10; // số mole spawn xung quanh 
    public GameObject finalPrefab; // prefab đặc biệt sau khi gộp 
    private bool spawningStopped = false;

    [Header("Audio Controller")]
    [SerializeField] private SoundData spawnSound;
    [SerializeField] private AudioSource audioSource;

    void Update()
    {
        if (spawningStopped)
        {
            Debug.Log("Stopped");
            return;
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {

            Debug.Log("Havent Stopped");
            SpawnMole();
            timer = 0f;
        }
    }

    void SpawnMole()
    {
        if (activeMoles.Count >= maxMoles)
        {
            int randomIndex = Random.Range(0, activeMoles.Count);
            Destroy(activeMoles[randomIndex]);
            activeMoles.RemoveAt(randomIndex);
        }

        Vector3 randomPos = new Vector3(
            Random.Range(spawnCenter.x - spawnRange.x / 2, spawnCenter.x + spawnRange.x / 2),
            spawnCenter.y,
            Random.Range(spawnCenter.z - spawnRange.z / 2, spawnCenter.z + spawnRange.z / 2)
        );

        GameObject mole = Instantiate(molePrefab, randomPos, Quaternion.identity);
        activeMoles.Add(mole);

        Mole moleScript = mole.GetComponent<Mole>();
        if (Random.value < 0.3f) // 30% cơ hội mole có chữ cái
        {
            moleScript.hasLetter = true;
            moleScript.letter = GameManager.Instance.GetNextNeededLetter();
        }
        PlaySound(spawnSound, audioSource);
    }

    public void HandleLevelComplete()
    {
        spawningStopped = true;
        if (activeMoles.Count == 0) return;
        GameObject centerMole = activeMoles[activeMoles.Count - 1];
        Vector3 centerPos = centerMole.transform.position;
        List<GameObject> spawnedAround = new List<GameObject>();
        for (int i = 0; i < specialSpawnCount; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 2f; offset.y = 0;
            GameObject mole = Instantiate(molePrefab, centerPos + offset, Quaternion.identity); spawnedAround.Add(mole);
        }
        foreach (var m in spawnedAround)
        {
            Destroy(m);
        }
        Destroy(centerMole);
        if (finalPrefab != null)
        {
            Instantiate(finalPrefab, centerPos, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnCenter, spawnRange);
    }

    private void PlaySound(SoundData data, AudioSource source)
    {
        if (data == null || source == null) return;

        source.clip = data.clip;                       // dùng đúng field clip
        source.outputAudioMixerGroup = data.mixer;     // dùng đúng field mixer
        source.volume = data.volume;
        source.loop = data.loop;
        source.pitch = Random.Range(data.pitchRange.x, data.pitchRange.y);

        if (data.is3D)
        {
            source.spatialBlend = 1f; // 3D
            source.minDistance = data.minDistance;
            source.maxDistance = data.maxDistance;
            source.rolloffMode = data.rolloff;
        }
        else
        {
            source.spatialBlend = 0f; // 2D
        }

        source.Play();
    }

}

