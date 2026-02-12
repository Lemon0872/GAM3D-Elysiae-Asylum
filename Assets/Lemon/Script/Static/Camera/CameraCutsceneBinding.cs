using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CameraCutsceneBinding
{
    public CameraCutsceneData data;
    public Transform target;
    [Header("Events")]
    public UnityEvent onArrived;
}
