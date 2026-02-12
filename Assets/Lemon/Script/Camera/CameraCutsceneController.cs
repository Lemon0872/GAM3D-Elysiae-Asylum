using UnityEngine;
using System.Collections;

public class CameraCutsceneController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera cutsceneCamera;
    public Camera mainCam;

    [Header("Optional")]
    public MonoBehaviour playerControllerToDisable;

    private ICameraCastMode smoothMode = new SmoothMoveMode();
    private ICameraCastMode teleportMode = new TeleportMode();

    private Coroutine currentRoutine;
    private CameraState originalState;

    void Awake()
    {
        if (cutsceneCamera != null)
            cutsceneCamera.gameObject.SetActive(false);
    }

    public void Cast(CameraCutsceneBinding binding)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(CastRoutine(binding));
    }

    private IEnumerator CastRoutine(CameraCutsceneBinding binding)
    {
        CameraCutsceneData data = binding.data;

        // 1️⃣ Enable cutscene cam
        cutsceneCamera.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);

        // 2️⃣ Lock player
        if (playerControllerToDisable != null)
            playerControllerToDisable.enabled = false;

        // 3️⃣ Save original state
        originalState = new CameraState(cutsceneCamera.transform);

        // 4️⃣ Execute mode
        ICameraCastMode mode =
            data.mode == CameraCastMode.SmoothMove
            ? smoothMode
            : teleportMode;

        yield return mode.Execute(cutsceneCamera, binding);

        // 5️⃣ Vừa đến vị trí → Invoke event
        binding.onArrived?.Invoke();

        // 6️⃣ RenderDuration (unscaled time)
        if (data.renderDuration > 0f)
        {
            float t = 0f;
            while (t < data.renderDuration)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        // 7️⃣ Restore camera
        originalState.Restore(cutsceneCamera.transform);

        // 8️⃣ Disable cutscene cam
        cutsceneCamera.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);

        // 9️⃣ Unlock player
        if (playerControllerToDisable != null)
            playerControllerToDisable.enabled = true;

        currentRoutine = null;
    }
}
