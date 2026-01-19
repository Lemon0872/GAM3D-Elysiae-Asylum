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

    [Header("Emission")]
    public Color emissionColor = Color.cyan;
    public float emissionIntensityMoving = 3.5f;
    public float emissionIntensityIdle = 0f;
    public float emissionLerpSpeed = 6f;

    public Renderer cubeRenderer;
    MaterialPropertyBlock mpb;
    Coroutine emissionRoutine;

    private void Start()
    {
        gridPos = GridUtility.WorldToGrid(transform.position);
        transform.position = GridUtility.GridToWorld(gridPos, cubeY);

        foreach (Tile t in FindObjectsOfType<Tile>())
            tileMap[t.gridPos] = t;

        lastGridPos = null;
        cubeRenderer = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void SetEmission(float intensity)
    {
        cubeRenderer.GetPropertyBlock(mpb, 0);
        mpb.SetColor("_EmissionColor", emissionColor * intensity);
        cubeRenderer.SetPropertyBlock(mpb, 0);
    }

    IEnumerator LerpEmission(float from, float to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * emissionLerpSpeed;
            float intensity = Mathf.Lerp(from, to, t);
            SetEmission(intensity);
            yield return null;
        }

        SetEmission(to);
    }

    void StartEmission()
    {
        if (emissionRoutine != null)
            StopCoroutine(emissionRoutine);

        emissionRoutine = StartCoroutine(
            LerpEmission(emissionIntensityIdle, emissionIntensityMoving)
        );
    }

    void StopEmission()
    {
        if (emissionRoutine != null)
            StopCoroutine(emissionRoutine);

        emissionRoutine = StartCoroutine(
            LerpEmission(emissionIntensityMoving, emissionIntensityIdle)
        );
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
        Debug.Log("Try teleport");

        if (isTeleporting || teleport == null || teleport.pairedTile == null)
        {
            state = CubeState.Idle;
            return;
        }

        // Không cho teleport nếu paired là chính nó
        if (teleport == teleport.pairedTile)
        {
            Debug.LogWarning("Teleport paired to itself");
            state = CubeState.Idle;
            return;
        }

        // Phải đi từ tile KHÁC sang teleport
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
        teleport.SpawnParticleAtTile();

        StartCoroutine(TeleportTo(teleport.pairedTile));
    }


    IEnumerator TeleportTo(TeleportTile target)
    {
        Debug.Log("Teleporting...");
        StartEmission();
        isTeleporting = true;
        state = CubeState.Moving;

        Vector2Int from = gridPos;

        yield return new WaitForSeconds(1f);

        lastGridPos = from;          
        gridPos = target.gridPos;     

        transform.position = GridUtility.GridToWorld(gridPos, cubeY);

        isTeleporting = false;
        state = CubeState.Idle;
        StopEmission();
        Debug.Log($"Teleport from {from} to {gridPos}");

        CheckTile(); 
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
        StartEmission();
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
        StopEmission();
    }

    void CheckTile()
    {
        if (!tileMap.TryGetValue(gridPos, out Tile tile))
        {
            state = CubeState.Idle;
            return;
        }
        tile.SpawnEnterFX();
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
