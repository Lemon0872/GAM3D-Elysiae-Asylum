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
    private bool isCasting;

    void Awake()
    {
        // Lưu state gameplay
        originalState = new CameraState(cutsceneCamera.transform);

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
        isCasting = true;

        if (playerControllerToDisable != null)
            playerControllerToDisable.enabled = false;

        // 🔥 Detach camera khỏi Player
        cutsceneCamera.transform.SetParent(null, true);

        cutsceneCamera.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);

        CameraCutsceneData data = binding.data;

        ICameraCastMode mode =
            data.mode == CameraCastMode.SmoothMove
            ? smoothMode
            : teleportMode;

        yield return mode.Execute(cutsceneCamera, binding);

        // RenderDuration
        if (data.moveDuration > 0f)
        {
            float t = 0f;
            while (t < data.moveDuration)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        BackCut();
    }

    void Update()
    {
        if (isCasting && Input.anyKeyDown)
            BackCut();
    }

    public void BackCut()
    {
        if (!isCasting) return;

        isCasting = false;

        cutsceneCamera.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);

        // 🔥 Restore parent + local transform gameplay
        originalState.Restore(cutsceneCamera.transform);

        if (playerControllerToDisable != null)
            playerControllerToDisable.enabled = true;

        currentRoutine = null;
    }
}