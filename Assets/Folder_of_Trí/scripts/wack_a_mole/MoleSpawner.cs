using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    public GameObject molePrefab;   // Prefab mole
    public Vector3 spawnCenter;     // Tọa độ trung tâm phạm vi spawn
    public Vector3 spawnRange;      // Kích thước phạm vi (x,y,z)

    public float spawnInterval = 2f; // Thời gian giữa các lần spawn
    private float timer;

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
        // Random vị trí trong phạm vi
        Vector3 randomPos = new Vector3(
            Random.Range(spawnCenter.x - spawnRange.x / 2, spawnCenter.x + spawnRange.x / 2),
            Random.Range(spawnCenter.y - spawnRange.y / 2, spawnCenter.y + spawnRange.y / 2),
            Random.Range(spawnCenter.z - spawnRange.z / 2, spawnCenter.z + spawnRange.z / 2)
        );

        GameObject mole = Instantiate(molePrefab, randomPos, Quaternion.identity);

        // Ví dụ: 30% cơ hội mole có chữ cái
        Mole moleScript = mole.GetComponent<Mole>();
        if (Random.value < 0.3f)
        {
            moleScript.hasLetter = true;
            moleScript.letter = GameManager.Instance.GetNextNeededLetter();
        }
    }

    // Vẽ gizmo để dễ thấy phạm vi spawn trong Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnCenter, spawnRange);
    }
}
