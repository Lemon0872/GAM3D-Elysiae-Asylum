using UnityEngine;

public class PieceDrag : MonoBehaviour
{
    Camera cam;
    BoardManager board;
    Piece piece;

    Vector3 offset;

    void Awake()
    {
        cam = Camera.main;
        board = FindObjectOfType<BoardManager>();
        piece = GetComponent<Piece>();
    }

    void OnMouseDown()
    {
        // Nếu đang placed thì gỡ khỏi board
        if (piece.IsPlaced)
        {
            board.RemovePiece(piece);
            piece.SetUnplaced();
        }

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        offset = transform.position - mouseWorld;
    }

    void OnMouseDrag()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        transform.position = mouseWorld + offset;
    }

    void OnMouseUp()
    {
        Vector2Int gridPos = board.WorldToGrid(transform.position);

        if (!board.IsInsideBoard(gridPos))
            return;

        if (board.CanPlace(piece, gridPos))
        {
            board.PlacePiece(piece, gridPos);
        }
    }
}