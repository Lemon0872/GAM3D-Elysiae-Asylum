using UnityEngine;

public enum CameraCastMode
{
    SmoothMove,
    Teleport
}

[CreateAssetMenu(menuName = "Camera/Cutscene Data")]
public class CameraCutsceneData : ScriptableObject
{
    public CameraCastMode mode;

    [Header("Camera Pose")]
    public Vector3 offset = new Vector3(0, 2, -5);

    [Header("Timing")]
    [Tooltip("Thời gian camera render. <= 0 = giữ camera, không auto disable")]
    public float renderDuration = 2f;

    [Tooltip("Chỉ dùng cho SmoothMove")]
    public float moveTime = 1f;
}
