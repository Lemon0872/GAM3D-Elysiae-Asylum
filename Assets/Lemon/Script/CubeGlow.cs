using UnityEngine;

public class CubeGlow : MonoBehaviour
{
    public bool isGlowing;
    public float emissionIntensity = 3f; // đúng bằng intensity HDR trong material gốc
    public Color emissionColor = Color.white; // hoặc màu gốc

    public Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        emissionColor = rend.material.GetColor("_EmissionColor");
    }

    void Update()
    {

    }

    public void SetGlow(bool state)
    {
        isGlowing = state;

        if (isGlowing)
            {
        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }
        else
        {
            rend.material.SetColor("_EmissionColor", Color.black);
            rend.material.DisableKeyword("_EMISSION");
        }
    }
}
