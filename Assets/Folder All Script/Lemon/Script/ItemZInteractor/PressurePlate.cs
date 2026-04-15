using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 0.5f;
    private Coroutine routine;
    [Header("Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    [SerializeField] public bool isPressed = false;
    [SerializeField] private Material materialA;
    [SerializeField] private Material materialB;
    [SerializeField] Renderer renderer;
    [SerializeField] Material runtimeMaterial;

    void Start()
    {
        renderer=this.GetComponent<Renderer>();
        runtimeMaterial=renderer.material;
    }
    public void Press()
    {
        isPressed = true;
        SetState(isPressed);
        if (animator != null) animator.SetBool("IsDown", true);
        onPressed?.Invoke();
    }

    public void Release()
    {
        isPressed = false;
        SetState(isPressed);
        if (animator != null) animator.SetBool("IsDown", false);
        onReleased?.Invoke();
    }
    public void SetState(bool useB)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Blend(useB ? materialB : materialA));
    }
    public void OnPress()
    {
        Debug.Log("da an");
    }
    public void OnRelease()
    {
        Debug.Log("da ve ban dau");
    }
    IEnumerator Blend(Material target)
    {
        float t = 0f;

        Color startColor = runtimeMaterial.GetColor("_Color");
        Color targetColor = target.GetColor("_Color");

        Color startEmission = runtimeMaterial.IsKeywordEnabled("_EMISSION")
            ? runtimeMaterial.GetColor("_EmissionColor")
            : Color.black;

        Color targetEmission = target.IsKeywordEnabled("_EMISSION")
            ? target.GetColor("_EmissionColor")
            : Color.black;

        float startMetallic = runtimeMaterial.GetFloat("_Metallic");
        float targetMetallic = target.GetFloat("_Metallic");

        float startSmoothness = runtimeMaterial.GetFloat("_Glossiness");
        float targetSmoothness = target.GetFloat("_Glossiness");

        while (t < 1f)
        {
            t += Time.deltaTime / transitionTime;

            runtimeMaterial.SetColor("_Color",
                Color.Lerp(startColor, targetColor, t));

            runtimeMaterial.SetColor("_EmissionColor",
                Color.Lerp(startEmission, targetEmission, t));

            runtimeMaterial.SetFloat("_Metallic",
                Mathf.Lerp(startMetallic, targetMetallic, t));

            runtimeMaterial.SetFloat("_Glossiness",
                Mathf.Lerp(startSmoothness, targetSmoothness, t));

            yield return null;
        }

        // đảm bảo khớp hoàn toàn
        runtimeMaterial.CopyPropertiesFromMaterial(target);
    }

    void OnCollisionExit(Collision collision)
{
    if (isPressed)
    {
        Release();
    }
}
}
