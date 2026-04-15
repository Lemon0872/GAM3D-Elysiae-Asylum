using UnityEngine;

public static class GridUtility
{
    public const float GRID_SIZE = 2f;

    public static Vector2Int WorldToGrid(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x / GRID_SIZE),
            Mathf.RoundToInt(pos.z / GRID_SIZE)
        );
    }

    public static Vector3 GridToWorld(Vector2Int grid, float y)
    {
        return new Vector3(
            grid.x * GRID_SIZE,
            y,
            grid.y * GRID_SIZE
        );
    }
}
