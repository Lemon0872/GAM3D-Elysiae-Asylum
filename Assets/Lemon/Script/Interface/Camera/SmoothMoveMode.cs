using UnityEngine;
using System.Collections;

public class SmoothMoveMode : ICameraCastMode
{
    public IEnumerator Execute(Camera cam, CameraCutsceneData data, Transform target)
    {
        Transform camTf = cam.transform;
        CameraState original = new CameraState(camTf);

        Vector3 targetPos = target.position + data.offset;

        yield return Move(camTf, targetPos, data.moveTime);
        camTf.LookAt(target);
    }

    IEnumerator Move(Transform cam, Vector3 target, float time)
    {
        float t = 0f;
        Vector3 start = cam.position;

        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            cam.position = Vector3.Lerp(start, target, t / time);
            yield return null;
        }
    }
}
