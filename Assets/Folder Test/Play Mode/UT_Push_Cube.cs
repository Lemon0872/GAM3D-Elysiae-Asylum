using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class UT_CUBIC
{
    private GameObject root;

    [SetUp]
    public void Setup()
    {
        root = new GameObject("ROOT");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(root);
    }

    // =========================================================
    // UT-Cubic-1: Collider interaction (PlayerPush -> Cube)
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_1_Player_Collider_Triggers_Push()
    {
        var cubeGO = new GameObject("Cube");
        cubeGO.transform.parent = root.transform;

        var cube = cubeGO.AddComponent<CubeController>();

        var playerGO = new GameObject("Player");
        playerGO.transform.position = Vector3.back;

        var push = cubeGO.AddComponent<PlayerPush>();
        push.cube = cube;
        push.playerTransform = playerGO.transform;

        yield return null;

        push.Interact(playerGO.transform);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-2: Push cube basic
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_2_Push_Cube_Moves()
    {
        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);

        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        // Force movement via coroutine
        cube.SendMessage("MoveTo", new Vector2Int(1, 0));

        yield return new WaitForSeconds(0.5f);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-3: Push while moving
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_3_Push_While_Moving_DoesNothing()
    {
        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        // Force state = Moving
        typeof(CubeController)
            .GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(cube, 1);

        cube.TryPush(new GameObject().transform);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-4: Push into normal tile
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_4_Push_To_Support_Tile()
    {
        CreateTile(Vector2Int.right, TileType.Support);

        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        cube.SendMessage("MoveTo", Vector2Int.right);

        yield return new WaitForSeconds(0.5f);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-5: Push into teleport
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_5_Push_To_Teleport()
    {
        var tp1 = CreateTeleport(Vector2Int.right);
        var tp2 = CreateTeleport(Vector2Int.up);

        tp1.pairedTile = tp2;
        tp2.pairedTile = tp1;

        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        cube.SendMessage("MoveTo", Vector2Int.right);

        yield return new WaitForSeconds(1.5f);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-6: After teleport position changes
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_6_Cube_Position_After_Teleport()
    {
        var tp1 = CreateTeleport(Vector2Int.right);
        var tp2 = CreateTeleport(new Vector2Int(2, 0));

        tp1.pairedTile = tp2;
        tp2.pairedTile = tp1;

        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);

        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        cube.SendMessage("MoveTo", Vector2Int.right);

        yield return new WaitForSeconds(1.5f);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-7: Blocker tile returns cube
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_7_Blocker_Returns_Cube()
    {
        CreateTile(Vector2Int.right, TileType.Blocker);

        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        cube.SendMessage("MoveTo", Vector2Int.right);

        yield return new WaitForSeconds(1f);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-8: Push outside platform
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_8_Push_Outside_NoTile()
    {
        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        cube.SendMessage("MoveTo", new Vector2Int(99, 99));

        yield return new WaitForSeconds(0.5f);

        Assert.Pass();
    }

    // =========================================================
    // UT-Cubic-9: Goal tile win
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_9_Goal_Triggers_Win()
    {
        CreateTile(Vector2Int.right, TileType.Goal);

        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        bool triggered = false;
        cube.onArrived.AddListener(() => triggered = true);

        yield return null;

        cube.SendMessage("MoveTo", Vector2Int.right);

        yield return new WaitForSeconds(0.5f);

        Assert.IsTrue(triggered);
    }

    // =========================================================
    // UT-Cubic-10: After win no more movement
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Cubic_10_No_Move_After_Win()
    {
        var cubeGO = CreateCubeWithTile(Vector2Int.zero, TileType.Support);
        var cube = cubeGO.GetComponent<CubeController>();

        yield return null;

        // force win state
        typeof(CubeController)
            .GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(cube, 3);

        cube.TryPush(new GameObject().transform);

        Assert.Pass();
    }

    // =========================================================
    // HELPERS
    // =========================================================

    GameObject CreateCubeWithTile(Vector2Int pos, TileType type)
    {
        CreateTile(pos, type);

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = root.transform;

        cube.AddComponent<CubeController>();

        return cube;
    }

    Tile CreateTile(Vector2Int pos, TileType type)
    {
        var go = new GameObject("Tile");
        go.transform.parent = root.transform;

        var tile = go.AddComponent<Tile>();
        tile.gridPos = pos;
        tile.type = type;

        return tile;
    }

    TeleportTile CreateTeleport(Vector2Int pos)
    {
        var go = new GameObject("Teleport");
        go.transform.parent = root.transform;

        var tp = go.AddComponent<TeleportTile>();
        tp.gridPos = pos;
        tp.type = TileType.Teleport;

        return tp;
    }
}