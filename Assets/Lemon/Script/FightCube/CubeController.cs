using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float cubeY = 1.2f;
    public float moveDuration = 0.3f;
    public float interactDistance = 1.6f;

    enum CubeState { Idle, Moving, Returning, Win }
    CubeState state = CubeState.Idle;

    Vector2Int gridPos;
    Vector2Int previousGridPos;

    Dictionary<Vector2Int, Tile> tileMap = new();
    Vector2Int? lastGridPos;   // tile trước đó (nullable)
    bool isTeleporting;

    Coroutine moveRoutine;
    Coroutine returnRoutine;


    private void Start()
    {
        gridPos = GridUtility.WorldToGrid(transform.position);
        transform.position = GridUtility.GridToWorld(gridPos, cubeY);

        foreach (Tile t in FindObjectsOfType<Tile>())
            tileMap[t.gridPos] = t;

        lastGridPos = null;
    }

    public void TryPush(Transform player)
    {
        if (state != CubeState.Idle) return;

        Vector2Int dir = PushDirectionResolver.GetDirection(
            transform.position,
            player.position,
            interactDistance
        );

        if (dir == Vector2Int.zero) return;

        Vector2Int? target = FindNextSupportedTile(gridPos, dir);

        if (!target.HasValue) return;

        previousGridPos = gridPos;
        moveRoutine = StartCoroutine(MoveTo(target.Value));
    }

    void TryTeleport(TeleportTile teleport)
    {
        Debug.Log("try tele");
        if (isTeleporting)
        {
            state = CubeState.Idle;
            return;
        }

        if (teleport == null || teleport.pairedTile == null)
        {
            Debug.LogWarning("Teleport tile missing pair");
            state = CubeState.Idle;
            return;
        }

        // Phải đi từ tile khác sang teleport
        if (!lastGridPos.HasValue || lastGridPos.Value == gridPos)
        {
            state = CubeState.Idle;
            return;
        }

        // Không cho teleport từ teleport → teleport
        if (tileMap.TryGetValue(lastGridPos.Value, out Tile prevTile) &&
            prevTile is TeleportTile)
        {
            state = CubeState.Idle;
            return;
        }
        StopMovementCoroutines();
        StartCoroutine(TeleportTo(teleport.pairedTile));
        Debug.Log($"Teleport from {gridPos} to {teleport.pairedTile.gridPos}");

    }

    IEnumerator TeleportTo(TeleportTile target)
    {
        Debug.Log("dang tele");

        isTeleporting = true;
        state = CubeState.Moving;

        yield return new WaitForSeconds(0.1f); // có thể bỏ nếu không cần FX

        lastGridPos = target.gridPos;
        gridPos = target.gridPos;

        transform.position = GridUtility.GridToWorld(gridPos, cubeY);

        isTeleporting = false;
        state = CubeState.Idle;
        Debug.Log("Teleport to " + target.gridPos);
    }

    Vector2Int? FindNextSupportedTile(Vector2Int start, Vector2Int dir)
    {
        Vector2Int check = start + dir;

        // Giới hạn an toàn để tránh loop vô hạn
        const int MAX_SCAN = 100;

        for (int i = 0; i < MAX_SCAN; i++)
        {
            // Gặp cube dẹp đầu tiên
            if (tileMap.ContainsKey(check))
            {
                return check;
            }

            check += dir;
        }

        // Không có cube dẹp nào phía trước
        return null;
    }

    IEnumerator MoveTo(Vector2Int target)
    {
        state = CubeState.Moving;

        Vector3 start = transform.position;
        Vector3 end = GridUtility.GridToWorld(target, cubeY);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        lastGridPos = gridPos;
        gridPos = target;
        CheckTile();
    }

    void CheckTile()
    {
        if (!tileMap.TryGetValue(gridPos, out Tile tile))
        {
            state = CubeState.Idle;
            return;
        }

        switch (tile.type)
        {
            case TileType.Support:
                state = CubeState.Idle;
                break;

            case TileType.Blocker:
                StartCoroutine(ReturnBack());
                break;

            case TileType.Goal:
                state = CubeState.Win;
                Debug.Log("WIN!");
                break;
            case TileType.Teleport:
                TryTeleport(tile as TeleportTile);
            break;
        }
    }

    void StopMovementCoroutines()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
    }

    IEnumerator ReturnBack()
    {
        state = CubeState.Returning;
        yield return new WaitForSeconds(0.2f);
        returnRoutine = StartCoroutine(MoveTo(previousGridPos));
    }
}
