using UnityEngine;
using System.Collections;

public class TeleportMode : ICameraCastMode
{
    public IEnumerator Execute(
        Camera cam,
        CameraCutsceneBinding binding
    )
    {
        Transform camTf = cam.transform;
        Transform lookTarget = binding.target;
        CameraCutsceneData data = binding.data;

        Vector3 targetWorldPos =
            lookTarget.position
            + lookTarget.forward * data.offset.z
            + lookTarget.up * data.offset.y
            + lookTarget.right * data.offset.x;

        camTf.position = targetWorldPos;
        camTf.LookAt(lookTarget);

        binding.onArrived?.Invoke();
        yield break;
    }
}