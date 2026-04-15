using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CameraCutsceneBinding
{
    public CameraCutsceneData data;
    public Transform target;
    [HideInInspector] public bool hasCast;
    public bool OnDisable;
    [Header("Events")]
    public UnityEvent onArrived;
}
