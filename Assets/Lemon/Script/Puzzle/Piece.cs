using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public List<PieceCell> cells = new List<PieceCell>();
    public PieceCell pivotCell;
    public static Piece SelectedPiece;
    private Vector3 dragOffset;
    private bool isDragging;
    private bool isRotating;

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
        if (isRotating) return;
        isRotating = true;

        ClearAllSlots();

        Vector3 pivot = pivotCell.transform.position;

        float targetZ = transform.eulerAngles.z - 90f;

        LeanTween.rotateAround(
            gameObject,
            Vector3.forward,
            -90f,
            0.25f
        ).setEase(LeanTweenType.easeOutCubic)
        .setOnComplete(() =>
        {
            isRotating = false;

            BoardManager.Instance.RecalculateCounts();
            TrySnap();
        });
    }

    void TrySnap()
    {
        BoardSlot pivotSlot =
            BoardManager.Instance.GetClosestSlot(
                pivotCell.transform.position
            );

        if (pivotSlot == null)
            return;

        Vector3 offset =
            pivotSlot.transform.position -
            pivotCell.transform.position;

        // Tính vị trí giả lập
        List<BoardSlot> targetSlots = new List<BoardSlot>();

        foreach (var cell in cells)
        {
            Vector3 futurePos = cell.transform.position + offset;

            BoardSlot slot =
                BoardManager.Instance.GetClosestSlot(futurePos);

            if (slot == null || slot.IsOccupied())
            {
                // Nếu bất kỳ slot nào bị chiếm → hủy snap
                return;
            }

            targetSlots.Add(slot);
        }

        // Nếu tới đây tức là tất cả hợp lệ

        transform.position += offset;

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].currentSlot = targetSlots[i];
            targetSlots[i].SetOccupied(cells[i]);
        }

        BoardManager.Instance.RecalculateCounts();
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
        BoardManager.Instance.RecalculateCounts();
    }
}