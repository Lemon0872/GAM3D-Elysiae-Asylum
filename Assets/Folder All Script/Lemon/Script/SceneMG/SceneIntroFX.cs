using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneIntroFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume volume;

    [Header("Timing")]
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private AnimationCurve curve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Lens Distortion")]
    [SerializeField] private float lensStart = -0.6f;
    [SerializeField] private float lensEnd = 0f;

    [Header("Chromatic Aberration")]
    [SerializeField] private float chromaStart = 0.3f;
    [SerializeField] private float chromaEnd = 0f;

    [Header("Vignette")]
    [SerializeField] private float vignetteStart = 0.3f;
    [SerializeField] private float vignetteEnd = 0f;
    [SerializeField] private ColorParameter vignetteColor;

    [Header("Volume Weight")]
    [SerializeField] private float weightStart = 1f;
    [SerializeField] private float weightEnd = 0f;

    LensDistortion lens;
    ChromaticAberration chroma;
    Vignette vignette;

    void Awake()
    {
        if (!volume)
        {
            Debug.LogError("[SceneIntroFX] Missing references!");
            enabled = false;
            return;
        }

        // Cache effects
        volume.profile.TryGet(out lens);
        volume.profile.TryGet(out chroma);
        volume.profile.TryGet(out vignette);

        // Safety override
        if (lens != null) lens.intensity.overrideState = true;
        if (chroma != null) chroma.intensity.overrideState = true;
        if (vignette != null) vignette.intensity.overrideState = true;
    }

    void Start()
    {
        // Init state
        volume.weight = weightStart;

        if (lens != null) lens.intensity.value = lensStart;
        if (chroma != null) chroma.intensity.value = chromaStart;
        if (vignette != null)
        {
            vignette.intensity.value = vignetteStart;
            vignette.color=vignetteColor;
        } 

        StartCoroutine(PlayIntro());
    }

    System.Collections.IEnumerator PlayIntro()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            float eased = curve.Evaluate(n);

            // Volume weight
            volume.weight =
                Mathf.Lerp(weightStart, weightEnd, eased);

            // Lens distortion
            if (lens != null)
                lens.intensity.value =
                    Mathf.Lerp(lensStart, lensEnd, eased);

            // Chromatic aberration
            if (chroma != null)
                chroma.intensity.value =
                    Mathf.Lerp(chromaStart, chromaEnd, eased);

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(vignetteStart, vignetteEnd, eased);

            yield return null;
        }

        // Final snap (precision)
        volume.weight = weightEnd;

        if (lens != null) lens.intensity.value = lensEnd;
        if (chroma != null) chroma.intensity.value = chromaEnd;
    }
}
