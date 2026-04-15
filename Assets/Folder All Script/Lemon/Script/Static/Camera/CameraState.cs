using UnityEngine;

[System.Serializable]
public struct CameraState
{
    public Transform parent;
    public Vector3 localPosition;
    public Quaternion localRotation;

    public CameraState(Transform cam)
    {
        parent = cam.parent;
        localPosition = cam.localPosition;
        localRotation = cam.localRotation;
    }

    public void Restore(Transform cam)
    {
        cam.SetParent(parent, false);
        cam.localPosition = localPosition;
        cam.localRotation = localRotation;
    }
}