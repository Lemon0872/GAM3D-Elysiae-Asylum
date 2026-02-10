using UnityEngine;

public struct CameraState
{
    public Vector3 position;
    public Quaternion rotation;

    public CameraState(Transform cam)
    {
        position = cam.position;
        rotation = cam.rotation;
    }

    public void Restore(Transform cam)
    {
        cam.position = position;
        cam.rotation = rotation;
    }
}
