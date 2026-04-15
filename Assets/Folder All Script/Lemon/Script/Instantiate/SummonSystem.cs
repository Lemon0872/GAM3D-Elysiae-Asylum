using UnityEngine;
using System.Collections.Generic;

public class SummonSystem : MonoBehaviour
{
    public static SummonSystem Instance;
    [SerializeField] private List<SummonEntry> summonEntries = new();
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Summon(int index)
    {
        if (index < 0 || index >= summonEntries.Count)
        {
            Debug.LogWarning($"Summon index {index} is out of range.");
            return;
        }

        SummonEntry entry = summonEntries[index];

        // Nếu đã summon rồi thì bỏ qua
        if (entry.hasSummoned)
        {
            Debug.Log($"Summon index {index} has already been used.");
            return;
        }

        // Đánh dấu đã summon
        entry.hasSummoned = true;

        // Spawn particle
        if (entry.particle != null)
        {
            GameObject particle = Instantiate(entry.particle, entry.spawnPosition, Quaternion.identity);

            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(particle, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        // Spawn prefab
        if (entry.prefab != null)
        {
            Instantiate(entry.prefab, entry.spawnPosition, Quaternion.identity);
        }
    }
}
