using UnityEngine;

public class TeleportTile : Tile
{
    [Header("Teleport")]
    public TeleportTile pairedTile;

    private void Reset()
    {
        type = TileType.Teleport;
    }
}
