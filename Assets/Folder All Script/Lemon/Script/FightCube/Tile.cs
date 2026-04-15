using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    public TileType type;
    public Vector2Int gridPos;

    [Header("FX")]
    public GameObject enterFXPrefab;
    void OnValidate()
    {
        UpdatePosition();
    }
    private void Awake()
    {
        
    }
    void UpdatePosition()=>transform.position = GridUtility.GridToWorld(gridPos, transform.position.y);

    public void SpawnEnterFX(float yOffset = 0.5f)
    {
        if (!enterFXPrefab) return;

        Vector3 pos = GridUtility.GridToWorld(
            gridPos,
            transform.position.y + yOffset
        );

        GameObject fx = Instantiate(enterFXPrefab, pos, Quaternion.identity);
        Destroy(fx, 2f);
    }
}
