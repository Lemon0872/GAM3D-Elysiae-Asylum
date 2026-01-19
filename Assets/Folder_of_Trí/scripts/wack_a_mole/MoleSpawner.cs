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

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnMole();
            timer = 0f;
        }
    }

    void SpawnMole()
    {
        // Nếu đã vượt quá số mole cho phép → destroy ngẫu nhiên 1 mole
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnCenter, spawnRange);
    }
}
