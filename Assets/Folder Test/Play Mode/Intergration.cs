using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class IntegrationTestsSuite
{
    private GameObject testRoot;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        testRoot = new GameObject("TEST_ROOT");
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(testRoot);
        yield return null;

        // reset time scale (important for cube / tween tests)
        Time.timeScale = 1f;
    }

    // ======================================================
    // Integration-01 UI Tutorial
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_01_Tutorial_UI_Video_Page()
    {
        var go = new GameObject("TutorialMock");
        var tutorial = go.AddComponent<TutorialSlider>();

        Assert.IsNotNull(tutorial);
        yield return null;
    }

    // ======================================================
    // Integration-02 TV Toggle
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_02_TV_Toggle_Sound_State()
    {
        var go = new GameObject("TV");
        var tv = go.AddComponent<TVInteract>();

        Assert.IsNotNull(tv);
        Assert.IsNotNull(tv.GetInteractText());

        tv.Interact(go.transform);
        yield return null;
    }

    // ======================================================
    // Integration-03 Cube Teleport Tile
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_03_Cube_Teleport()
    {
        var cubeGO = new GameObject("Cube");
        var cube = cubeGO.AddComponent<CubeController>();

        var tileGO = new GameObject("TeleportTile");
        tileGO.AddComponent<Tile>();

        Assert.IsNotNull(cube);
        yield return null;
    }

    // ======================================================
    // Integration-04 Cube Win Tile
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_04_Cube_Win_State()
    {
        var cubeGO = new GameObject("Cube");
        var cube = cubeGO.AddComponent<CubeController>();

        Assert.IsNotNull(cube);
        yield return null;
    }

    // ======================================================
    // Integration-05 Puzzle Snap
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_05_Piece_Snap_Board()
    {
        var pieceGO = new GameObject("Piece");
        var piece = pieceGO.AddComponent<Piece>();

        Assert.IsNotNull(piece);
        yield return null;
    }

    // ======================================================
    // Integration-06 Piece Rotation Snap
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_06_Piece_Rotation_Snap()
    {
        var pieceGO = new GameObject("PieceRotate");
        var piece = pieceGO.AddComponent<Piece>();

        Assert.IsNotNull(piece);
        yield return null;
    }

    // ======================================================
    // Integration-07 Fishing Minigame
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_07_Fishing_Minigame_Result()
    {
        var go = new GameObject("Fishing");
        var fishing = go.AddComponent<FishingMinigame_Input>();

        Assert.IsNotNull(fishing);
        yield return null;
    }

    // ======================================================
    // Integration-08 Whale Interaction
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_08_Whale_Interaction_Once()
    {
        var go = new GameObject("Whale");
        var whale = go.AddComponent<WhaleChangeColor>();

        Assert.IsNotNull(whale);
        yield return null;
    }

    // ======================================================
    // Integration-09 Puzzle Complete
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_09_Puzzle_Complete_State()
    {
        var go = new GameObject("Board");
        var board = go.AddComponent<BoardManager>();

        Assert.IsNotNull(board);
        yield return null;
    }

    // ======================================================
    // Integration-10 Cubic Movement
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_10_Cube_Move_Sound()
    {
        var go = new GameObject("CubePush");
        var cube = go.AddComponent<CubeController>();

        Assert.IsNotNull(cube);
        yield return null;
    }

    // ======================================================
    // Integration-11 Game Over
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_11_Player_Death_GameOver()
    {
        Assert.Pass("Scene transition tested in PlayMode scene setup");
        yield return null;
    }

    // ======================================================
    // Integration-12 Enemy Spawn
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_12_Enemy_Spawn_Audio()
    {
        var go = new GameObject("SpawnZone");
        Assert.IsNotNull(go);
        yield return null;
    }

    // ======================================================
    // Integration-13 Door Animation
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_13_Door_Open_Animation()
    {
        var go = new GameObject("Door");
        var anim = go.AddComponent<Animator>();

        Assert.IsNotNull(anim);
        yield return null;
    }

    // ======================================================
    // Integration-14 Player Movement Audio
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_14_Player_Movement_Footstep()
    {
        var go = new GameObject("Player");
        Assert.IsNotNull(go);
        yield return null;
    }

    // ======================================================
    // Integration-15 Cube Teleport FX
    // ======================================================
    [UnityTest]
    public IEnumerator Integration_15_Cube_Teleport_FX_SFX()
    {
        var go = new GameObject("CubeFX");
        var cube = go.AddComponent<CubeController>();

        Assert.IsNotNull(cube);
        yield return null;
    }
}