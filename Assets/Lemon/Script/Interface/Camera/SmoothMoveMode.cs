using UnityEngine;
using System.Collections;

public class SmoothMoveMode : ICameraCastMode
{
    public IEnumerator Execute(Camera cam, CameraCutsceneBinding binding)
    {
        Transform camTf = cam.transform;
        CameraCutsceneData data = binding.data;
        Transform lookTarget = binding.target;

        // 🔥 1️⃣ WORLD position mong muốn
        Vector3 targetWorldPos = lookTarget.position + data.offset;

        // 🔥 2️⃣ Convert sang LOCAL của parent hiện tại
        Transform parent = camTf.parent;

        Vector3 targetLocalPos =
            parent.InverseTransformPoint(targetWorldPos);

        yield return MoveLocal(
            camTf,
            targetLocalPos,
            lookTarget,
            data.moveTime
        );

        // 🔒 Snap cuối để tránh lệch do float
        camTf.localPosition = targetLocalPos;

        camTf.rotation = Quaternion.LookRotation(
            lookTarget.position - camTf.position,
            Vector3.up
        );
    }

    IEnumerator MoveLocal(
        Transform cam,
        Vector3 targetLocalPos,
        Transform lookTarget,
        float time
    )
    {
        float t = 0f;
        Vector3 startLocalPos = cam.localPosition;

        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / time);

            cam.localPosition =
                Vector3.Lerp(startLocalPos, targetLocalPos, lerp);

            // LookAt bằng WORLD SPACE
            Vector3 dir = lookTarget.position - cam.position;
            cam.rotation = Quaternion.LookRotation(dir, Vector3.up);

            yield return null;
        }
    }
}
