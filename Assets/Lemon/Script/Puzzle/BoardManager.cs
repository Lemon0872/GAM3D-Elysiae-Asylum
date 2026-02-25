using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [Header("Board Settings")]
    [Range(4, 6)]
    public int boardSize = 4;
    public float cellSize = 1f;
    public float snapDistance = 0.5f;

    [Header("Win Condition")]
    public List<string> requiredSlotIDs = new List<string>();

    private Dictionary<string, BoardSlot> slotDictionary =
        new Dictionary<string, BoardSlot>();

    private List<BoardSlot> requiredSlots =
        new List<BoardSlot>();

    private void Awake()
    {
        Instance = this;
        GenerateBoard();
        CacheRequiredSlots();
    }

    void GenerateBoard()
    {
        slotDictionary.Clear();

        float half = (boardSize - 1) * cellSize * 0.5f;

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                string id = $"{(char)('A' + col)}{row + 1}";

                GameObject slotObj = new GameObject(id);
                slotObj.transform.parent = transform;

                float x = col * cellSize - half;
                float y = -row * cellSize + half;

                slotObj.transform.localPosition = new Vector3(x, y, 0);

                BoardSlot slot = slotObj.AddComponent<BoardSlot>();
                slot.Initialize(id);

                slotDictionary.Add(id, slot);
            }
        }
    }

    void CacheRequiredSlots()
    {
        requiredSlots.Clear();

        foreach (var id in requiredSlotIDs)
        {
            if (slotDictionary.TryGetValue(id, out BoardSlot slot))
            {
                requiredSlots.Add(slot);
                slot.isRequired = true;
            }
        }
    }

    public BoardSlot GetClosestSlot(Vector3 worldPos)
    {
        float minDist = snapDistance;
        BoardSlot closest = null;

        foreach (var slot in slotDictionary.Values)
        {
            float dist = Vector3.Distance(worldPos, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = slot;
            }
        }

        return closest;
    }

    public void CheckWin()
    {
        Debug.Log("thuc hien checkwin");
        foreach (var slot in requiredSlots)
        {
            if (!slot.IsOccupied())
                return;
        }

        Debug.Log("LEVEL COMPLETE");
    }
    #if UNITY_EDITOR
void OnDrawGizmos()
{
    Gizmos.color = Color.aquamarine;

    float totalSize = boardSize * cellSize;
    float half = totalSize * 0.5f;

    Vector3 origin = transform.position - 
                     new Vector3(half, -half, 0);

    // Dọc
    for (int col = 0; col <= boardSize; col++)
    {
        Vector3 start = origin + new Vector3(col * cellSize, 0, 0);
        Vector3 end = start + new Vector3(0, -totalSize, 0);
        Gizmos.DrawLine(start, end);
    }

    // Ngang
    for (int row = 0; row <= boardSize; row++)
    {
        Vector3 start = origin + new Vector3(0, -row * cellSize, 0);
        Vector3 end = start + new Vector3(totalSize, 0, 0);
        Gizmos.DrawLine(start, end);
    }
}
#endif
}