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
            lookTarget.TransformPoint(data.offset);

        Vector3 targetLocalPos =
            camTf.parent.InverseTransformPoint(targetWorldPos);

        camTf.localPosition = targetLocalPos;
        camTf.rotation = Quaternion.LookRotation(
            lookTarget.position - camTf.position,
            Vector3.up
        );

        // 👇 Gọi event ngay lập tức
        binding.onArrived?.Invoke();

        yield break;
    }
}

