using UnityEngine;

public class TeleportTile : Tile
{
    [Header("Teleport")]
    public TeleportTile pairedTile;
    public GameObject teleportParticlePrefab;
    private void Reset()
    {
        type = TileType.Teleport;
    }
    public void SpawnParticleAtTile()
{
    if (!teleportParticlePrefab) return;

    Vector3 pos = GridUtility.GridToWorld(
        gridPos,
        transform.position.y + 1f
    );

    GameObject fx = Instantiate(
        teleportParticlePrefab,
        pos,
        Quaternion.identity
    );

    Destroy(fx, 1.82f);
}
}
