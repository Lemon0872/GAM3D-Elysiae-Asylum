using UnityEngine;
using System.Collections;

public class SmoothMoveMode : ICameraCastMode
{
    public IEnumerator Execute(
        Camera cam,
        CameraCutsceneBinding binding
    )
    {
        Transform camTf = cam.transform;
        Transform lookTarget = binding.target;
        CameraCutsceneData data = binding.data;

        Vector3 startPos = camTf.position;
        Quaternion startRot = camTf.rotation;

        Vector3 targetWorldPos =
            lookTarget.position
            + lookTarget.forward * data.offset.z
            + lookTarget.up * data.offset.y
            + lookTarget.right * data.offset.x;

        Quaternion targetRot =
            Quaternion.LookRotation(
                lookTarget.position - targetWorldPos,
                Vector3.up
            );

        float t = 0f;

        while (t < data.moveDuration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = t / data.moveDuration;

            camTf.position = Vector3.Lerp(startPos, targetWorldPos, lerp);
            camTf.rotation = Quaternion.Slerp(startRot, targetRot, lerp);

            yield return null;
        }

        camTf.position = targetWorldPos;
        camTf.rotation = targetRot;

        binding.onArrived?.Invoke();
    }
}