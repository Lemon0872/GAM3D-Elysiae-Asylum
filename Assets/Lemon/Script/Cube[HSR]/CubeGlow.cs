using UnityEngine;

public class CubeGlow : MonoBehaviour
{
    public bool isGlowing;
    public float emissionIntensity = 3f; // đúng bằng intensity HDR trong material gốc
    public Color emissionColor = Color.white; // hoặc màu gốc

    public Renderer rend;
    private bool isGlowSound;

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
            if (!isGlowSound)
            {
                AudioManager.PlaySFXAt("Cube[HSR]Glow", transform.position);
                isGlowSound=true;
            } 
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", emissionColor * emissionIntensity);
        }
        else
        {
            isGlowSound=false;
            rend.material.SetColor("_EmissionColor", Color.black);
            rend.material.DisableKeyword("_EMISSION");
        }
    }
}
