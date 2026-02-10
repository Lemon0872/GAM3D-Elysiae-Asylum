using UnityEngine;
using System.Collections;

public class TeleportMode : ICameraCastMode
{
    public IEnumerator Execute(Camera cam, CameraCutsceneData data, Transform target)
    {
        Transform camTf = cam.transform;

        camTf.position = target.position + data.offset;
        camTf.LookAt(target);
        yield break;
    }
}
