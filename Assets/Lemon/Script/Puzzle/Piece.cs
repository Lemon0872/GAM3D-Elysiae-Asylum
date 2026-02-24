using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Data")]
    public PieceShapeData shapeData;

    [Header("Reference")]
    public GameObject cellPrefab;
    private Vector2Int pivotCell;
    public bool IsPlaced { get; private set; }
    public Vector2Int CurrentGridPos { get; private set; }

    void Start()
    {
        BuildPiece();
    }

    void BuildPiece()
    {
        if (shapeData == null) return;

        pivotCell = shapeData.cells[shapeData.pivotIndex];

        foreach (Vector2Int cell in shapeData.cells)
        {
            Vector2Int localPos = cell - pivotCell;

            GameObject cellObj = Instantiate(cellPrefab, transform);

            cellObj.transform.localPosition = new Vector3(
                localPos.x,
                localPos.y,
                0f
            );
        }
    }

    // Lấy tất cả cell đang chiếm sau khi snap
    public Vector2Int[] GetOccupiedCells(Vector2Int gridPosition)
    {
        Vector2Int[] result = new Vector2Int[shapeData.cells.Length];

        for (int i = 0; i < shapeData.cells.Length; i++)
        {
            result[i] = gridPosition + (shapeData.cells[i] - pivotCell);
        }

        return result;
    }
    
    public void SetPlacedPosition(Vector2Int gridPos)
    {
        CurrentGridPos = gridPos;
        IsPlaced = true;
    }

    public void SetUnplaced()
    {
        IsPlaced = false;
    }
}