using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public List<PieceCell> cells = new List<PieceCell>();
    public PieceCell pivotCell;
    public static Piece SelectedPiece;
    private Vector3 dragOffset;
    private bool isDragging;

    void Start()
    {
        cells.AddRange(GetComponentsInChildren<PieceCell>());
    }

    void Update()
    {
        if (SelectedPiece == this && Input.GetKeyDown(KeyCode.R))
        {
            RotatePiece();
            
        }
    }

    void OnMouseDown()
    {
        SelectedPiece = this;
        isDragging = true;
        dragOffset = transform.position - GetMouseWorld();
        ClearAllSlots();
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        transform.position = GetMouseWorld() + dragOffset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        TrySnap();
    }

    Vector3 GetMouseWorld()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    void RotatePiece()
    {
        ClearAllSlots();
        transform.RotateAround(
            pivotCell.transform.position,
            Vector3.forward,
            -90f
        );
        TrySnap();
    }

    void TrySnap()
    {
        BoardSlot closest =
            BoardManager.Instance.GetClosestSlot(
                pivotCell.transform.position
            );

        if (closest == null)
            return;

        Vector3 offset =
            closest.transform.position -
            pivotCell.transform.position;

        transform.position += offset;

        OccupySlots();
        BoardManager.Instance.CheckWin();
    }

    void OccupySlots()
    {
        foreach (var cell in cells)
        {
            BoardSlot slot =
                BoardManager.Instance.GetClosestSlot(
                    cell.transform.position
                );

            if (slot != null)
            {
                cell.currentSlot = slot;
                slot.SetOccupied(cell);
            }
        }
    }

    void ClearAllSlots()
    {
        foreach (var cell in cells)
        {
            if (cell.currentSlot != null)
            {
                cell.currentSlot.Clear();
                cell.currentSlot = null;
            }
        }
    }
}