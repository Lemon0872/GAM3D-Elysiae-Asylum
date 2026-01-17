using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    public TileType type;
    public Vector2Int gridPos;

    void OnValidate()
    {
        UpdatePosition();
    }
    private void Awake()
    {
        
    }
    void UpdatePosition()=>transform.position = GridUtility.GridToWorld(gridPos, transform.position.y);
}
