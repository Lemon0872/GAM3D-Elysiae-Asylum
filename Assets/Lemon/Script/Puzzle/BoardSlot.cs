using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public string ID;
    public bool isRequired;
    private PieceCell occupiedCell;

    public void Initialize(string id)
    {
        ID = id;
    }

    public bool IsOccupied()
    {
        return occupiedCell != null;
    }

    public void SetOccupied(PieceCell cell)
    {
        occupiedCell = cell;
    }

    public void Clear()
    {
        occupiedCell = null;
    }
}