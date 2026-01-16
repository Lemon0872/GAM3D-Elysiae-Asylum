using UnityEngine;

public class CubePiece : MonoBehaviour
{
    public Vector3Int logicalPosition; // -1, 0, 1
    public bool isColored = false;

    private Renderer rend;
    private Color baseColor = Color.cyan;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateVisual();
    }

    public void SetColored(bool state)
    {
        isColored = state;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (!rend) rend = GetComponent<Renderer>();

        Color c = baseColor;
        if (isColored)
        {
            c.a = 1f; // đậm màu
        }
        else
        {
            c.a = 0.2f; // trong suốt
        }
        rend.material.color = c;
    }
}
