using UnityEngine;

public static class PushDirectionResolver
{
    public static Vector2Int GetDirection(
        Vector3 cubePos,
        Vector3 playerPos,
        float maxDistance
    )
    {
        Vector3 dir = playerPos - cubePos;
        dir.y = 0;

        if (dir.magnitude > maxDistance || dir.sqrMagnitude < 0.01f)
            return Vector2Int.zero;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle >= -45f && angle < 45f)
            return Vector2Int.left;   // player ở +X

        if (angle >= 45f && angle < 135f)
            return Vector2Int.down;   // player ở +Z

        if (angle >= 135f || angle < -135f)
            return Vector2Int.right;  // player ở -X

        return Vector2Int.up;         // player ở -Z
    }
}
