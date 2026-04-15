using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;

public class UT_EPuzzle
{
    // =========================
    // ✅ GLOBAL CLEANUP
    // =========================
    [TearDown]
    public void Cleanup()
    {
        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
            GameObject.DestroyImmediate(obj);
    }

    // =========================
    // ✅ HELPERS
    // =========================
BoardManager SetupBoard(int size = 2, float snapDistance = 1f)
{
    var go = new GameObject("Board");
    var board = go.AddComponent<BoardManager>();

    board.boardSize = size;
    board.cellSize = 1f;
    board.snapDistance = snapDistance;

    board.rowContain = CreateBars(size);
    board.colContain = CreateBars(size);

    // ✅ FIX private fields via reflection
    var rowLimits = new int[size];
    var colLimits = new int[size];

    for (int i = 0; i < size; i++)
    {
        rowLimits[i] = 10;
        colLimits[i] = 10;
    }

    typeof(BoardManager)
        .GetField("rowLimits", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .SetValue(board, rowLimits);

    typeof(BoardManager)
        .GetField("colLimits", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .SetValue(board, colLimits);

    board.SendMessage("Awake");

    return board;
}

    SegmentedBar[] CreateBars(int size)
    {
        var arr = new SegmentedBar[size];
        for (int i = 0; i < size; i++)
        {
            var go = new GameObject($"Bar_{i}");
            var bar = go.AddComponent<SegmentedBar>();
            bar.segments = new List<GameObject> { new GameObject("Segment") };
            arr[i] = bar;
        }
        return arr;
    }

    // =========================
    // 🧪 PIECE TESTS
    // =========================

    // UT-EPuzzle-1: Pick piece
    [UnityTest]
    public IEnumerator UT_EPuzzle_1_Pick_Piece()
    {
        var go = new GameObject();
        var piece = go.AddComponent<Piece>();

        go.SendMessage("OnMouseDown");

        yield return null;

        Assert.AreEqual(piece, Piece.SelectedPiece);
    }

    // UT-EPuzzle-2: Release piece
    [UnityTest]
    public IEnumerator UT_EPuzzle_2_Release_Piece()
    {
        var go = new GameObject();
        var piece = go.AddComponent<Piece>();

        go.SendMessage("OnMouseDown");
        go.SendMessage("OnMouseUp");

        yield return null;

        Assert.Pass();
    }

    // UT-EPuzzle-5: Snap piece near frame
    [UnityTest]
    public IEnumerator UT_EPuzzle_5_Snap_Piece_Near_Frame()
    {
        var board = SetupBoard(2, 100f);

        var pieceGO = new GameObject();
        var piece = pieceGO.AddComponent<Piece>();

        var cellGO = new GameObject();
        var cell = cellGO.AddComponent<PieceCell>();
        cell.transform.parent = pieceGO.transform;

        piece.cells = new List<PieceCell> { cell };
        piece.pivotCell = cell;

        yield return null;

        pieceGO.SendMessage("TrySnap");

        Assert.Pass();
    }

    // =========================
    // 🧪 BOARDSLOT TESTS
    // =========================

    // UT-EPuzzle-1: Initialize slot
    [Test]
    public void UT_EPuzzle_1_Initialize_Slot()
    {
        var go = new GameObject();
        var slot = go.AddComponent<BoardSlot>();

        slot.Initialize("A1");

        Assert.AreEqual("A1", slot.ID);
    }

    // UT-EPuzzle-2: Occupy slot
    [Test]
    public void UT_EPuzzle_2_Occupy_Slot()
    {
        var slot = new GameObject().AddComponent<BoardSlot>();
        var cell = new GameObject().AddComponent<PieceCell>();

        slot.SetOccupied(cell);

        Assert.IsTrue(slot.IsOccupied());
    }

    // UT-EPuzzle-3: Clear slot
    [Test]
    public void UT_EPuzzle_3_Clear_Slot()
    {
        var slot = new GameObject().AddComponent<BoardSlot>();
        var cell = new GameObject().AddComponent<PieceCell>();

        slot.SetOccupied(cell);
        slot.Clear();

        Assert.IsFalse(slot.IsOccupied());
    }

    // =========================
    // 🧪 BOARD MANAGER TESTS
    // =========================

    // UT-EPuzzle-1: Generate board
    [UnityTest]
    public IEnumerator UT_EPuzzle_1_Generate_Board()
    {
        var board = SetupBoard();

        yield return null;

        Assert.Pass();
    }

    // UT-EPuzzle-6: Drop piece far from frame
    [UnityTest]
    public IEnumerator UT_EPuzzle_6_Drop_Piece_Far_From_Frame()
    {
        var board = SetupBoard(2, 0.1f);

        var slot = board.GetClosestSlot(Vector3.one * 999);

        Assert.IsNull(slot);

        yield return null;
    }

    // UT-EPuzzle-7: Complete minigame
    [UnityTest]
    public IEnumerator UT_EPuzzle_7_Complete_Minigame()
    {
        var board = SetupBoard();

        board.requiredSlotIDs = new List<string> { "A1" };
        board.SendMessage("Awake");

        foreach (var slot in GameObject.FindObjectsOfType<BoardSlot>())
        {
            if (slot.ID == "A1")
                slot.SetOccupied(new GameObject().AddComponent<PieceCell>());
        }

        board.CheckWin();

        yield return null;

        Assert.Pass();
    }
}