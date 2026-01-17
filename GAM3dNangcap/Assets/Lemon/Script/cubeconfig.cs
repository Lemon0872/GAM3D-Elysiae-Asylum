using UnityEngine;

[ExecuteAlways] // Cho phép hoạt động cả trong chế độ Edit mode
public class cubeconfig : MonoBehaviour
{
    [Header("Cube Configuration")]
    public bool isLunk = false; // ✅ Tích chọn trong Inspector để bật/tắt màu
    public Material lunkMaterial;    // Material màu
    public Material nonLunkMaterial; // Material trong suốt

    private Renderer cubeRenderer;
    void Awake()
    {
        ApplyConfiguration();
    }
    void OnValidate()
    {
        // ✅ Cập nhật tag, material, và symbol
        if (isLunk)
        {
            cubeRenderer.sharedMaterial = lunkMaterial;
            gameObject.tag = "Lunk";
        }
        else
        {
            cubeRenderer.sharedMaterial = nonLunkMaterial;
            gameObject.tag = "Non-Lunk";
        }
    }
    void Start()
    {
        
    }
    public void ApplyConfiguration()
    {
        if (cubeRenderer == null)
            cubeRenderer = GetComponent<Renderer>();

        if (cubeRenderer == null) return;

        
    }
}
