using UnityEngine;

public class CubeGlow : MonoBehaviour
{
    public bool isGlowing;
    public float emissionIntensity = 3f; // đúng bằng intensity HDR trong material gốc
    public Color emissionColor = Color.white; // hoặc màu gốc

    private Renderer rend;
    private Material mat;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = Instantiate(rend.sharedMaterial); // clone giữ nguyên shader
        rend.material = mat;
    }
    void Update()
    {

    }

    public void SetGlow(bool state)
    {
        isGlowing = state;
        if (isGlowing)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }
}
