using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Size (Square)")]
    public int size = 4;

    [Header("Required Cells Per Row")]
    public int[] requiredPerRow;

    [Header("Required Cells Per Column")]
    public int[] requiredPerColumn;

    private bool[,] occupied;

    float OriginOffset => -size / 2f;

    void Awake()
    {
        occupied = new bool[size, size];
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (size < 1) size = 1;

        ResizeArray(ref requiredPerRow);
        ResizeArray(ref requiredPerColumn);
    }

    void ResizeArray(ref int[] array)
    {
        if (array == null || array.Length != size)
        {
            int[] newArray = new int[size];

            if (array != null)
            {
                for (int i = 0; i < Mathf.Min(array.Length, size); i++)
                    newArray[i] = array[i];
            }

            array = newArray;
        }
    }
#endif

    // =========================
    // WORLD → GRID
    // =========================
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        float x = worldPos.x - OriginOffset;
        float y = worldPos.y - OriginOffset;

        return new Vector2Int(
            Mathf.FloorToInt(x),
            Mathf.FloorToInt(y)
        );
    }

    // =========================
    // GRID → WORLD
    // =========================
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float x = gridPos.x + OriginOffset + 0.5f;
        float y = gridPos.y + OriginOffset + 0.5f;

        return new Vector3(x, y, 0f);
    }

    public bool IsInsideBoard(Vector2Int gridPos)
    {
        return gridPos.x >= 0 &&
               gridPos.y >= 0 &&
               gridPos.x < size &&
               gridPos.y < size;
    }

    public bool CanPlace(Piece piece, Vector2Int gridPos)
    {
        var cells = piece.GetOccupiedCells(gridPos);

        foreach (var cell in cells)
        {
            if (!IsInsideBoard(cell))
                return false;

            if (occupied[cell.x, cell.y])
                return false;
        }

        return true;
    }

    public void PlacePiece(Piece piece, Vector2Int gridPos)
    {
        var cells = piece.GetOccupiedCells(gridPos);

        foreach (var cell in cells)
            occupied[cell.x, cell.y] = true;

        piece.transform.position = GridToWorld(gridPos);
        piece.SetPlacedPosition(gridPos);

        CheckWin();
    }

    public void RemovePiece(Piece piece)
    {
        if (!piece.IsPlaced)
            return;

        var cells = piece.GetOccupiedCells(piece.CurrentGridPos);

        foreach (var cell in cells)
            occupied[cell.x, cell.y] = false;
    }

    // =========================
    // CHECK WIN CONDITION
    // =========================
    void CheckWin()
    {
        // Check rows
        for (int y = 0; y < size; y++)
        {
            int count = 0;
            for (int x = 0; x < size; x++)
                if (occupied[x, y]) count++;

            if (count != requiredPerRow[y])
                return;
        }

        // Check columns
        for (int x = 0; x < size; x++)
        {
            int count = 0;
            for (int y = 0; y < size; y++)
                if (occupied[x, y]) count++;

            if (count != requiredPerColumn[x])
                return;
        }

        Debug.Log("WIN!");
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.white;

    //     float originOffset = -size / 2f;

    //     for (int x = 0; x <= size; x++)
    //     {
    //         float xPos = originOffset + x;

    //         Gizmos.DrawLine(
    //             new Vector3(xPos, originOffset, 0),
    //             new Vector3(xPos, originOffset + size, 0)
    //         );
    //     }

    //     for (int y = 0; y <= size; y++)
    //     {
    //         float yPos = originOffset + y;

    //         Gizmos.DrawLine(
    //             new Vector3(originOffset, yPos, 0),
    //             new Vector3(originOffset + size, yPos, 0)
    //         );
    //     }
    // }
}