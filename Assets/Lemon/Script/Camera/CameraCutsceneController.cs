using UnityEngine;
using System.Collections;

public class CameraCutsceneController : MonoBehaviour
{
    [Header("Overlay Camera")]
    public Camera cutsceneCamera;

    private ICameraCastMode smoothMode = new SmoothMoveMode();
    private ICameraCastMode teleportMode = new TeleportMode();

    private Coroutine currentRoutine;
    private CameraState originalState;

    void Awake()
    {
        if (cutsceneCamera != null)
            cutsceneCamera.gameObject.SetActive(false);
    }

    public void Cast(CameraCutsceneData data, Transform target)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(CastRoutine(data, target));
    }

    private IEnumerator CastRoutine(CameraCutsceneData data, Transform target)
    {
        // 1️⃣ Enable camera
        cutsceneCamera.gameObject.SetActive(true);

        // 2️⃣ Save state
        originalState = new CameraState(cutsceneCamera.transform);

        // 3️⃣ Execute mode
        ICameraCastMode mode =
            data.mode == CameraCastMode.SmoothMove
            ? smoothMode
            : teleportMode;

        yield return mode.Execute(cutsceneCamera, data, target);

        // 4️⃣ Render trong thời gian quy định (unscaled)
        if (data.renderDuration > 0f)
        {
            float t = 0f;
            while (t < data.renderDuration)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            // 5️⃣ Restore & disable
            originalState.Restore(cutsceneCamera.transform);
            cutsceneCamera.gameObject.SetActive(false);
        }

        currentRoutine = null;
    }
}
