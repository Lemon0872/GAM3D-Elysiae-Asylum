using UnityEngine;

public class LeafPicker : MonoBehaviour
{
    public float pickRange = 3f;           // khoảng cách nhặt lá
    public LayerMask leafLayer;            // layer dành cho lá

    void Update()
    {
        if (Input.GetMouseButtonDown(0))   // click chuột trái
        {
            // Ray từ giữa màn hình (viewport center)
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, pickRange, leafLayer))
            {
                // Nếu collider có tag "Leaf"
                if (hit.collider.CompareTag("Leaf"))
                {
                    // Cộng điểm và xóa lá
                    LeafManager.Instance.AddLeaf(1);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
