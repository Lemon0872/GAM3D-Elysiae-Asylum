using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/Piece Shape Data")]
public class PieceShapeData : ScriptableObject
{
    [Header("Shape Info")]
    public string shapeName;
    [Header("Shape Layout")]
    public Vector2Int[] cells; 
    // Tọa độ các ô tương đối

    [Header("Pivot Cell Index")]
    public int pivotIndex = 0;
    // Ô nào sẽ làm pivot
}