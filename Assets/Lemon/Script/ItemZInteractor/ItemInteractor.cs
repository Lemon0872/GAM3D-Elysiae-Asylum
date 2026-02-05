using UnityEngine;

public class ItemInteractor : MonoBehaviour,IInteractable
{
    [SerializeField] bool isHeld;
    [SerializeField] private string interactText;
    [SerializeField] Transform holder;
    [SerializeField] float placeDistance;
    [SerializeField] private PressurePlate pressurePlate;
    [SerializeField] private string plateZTag = "PlateZ";

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    void IInteractable.Interact(Transform interactorTransform)
    {
        PickUp(holder);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(1)&&isHeld) Place();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(plateZTag)) return;
        pressurePlate = collision.gameObject.GetComponent<PressurePlate>();
        if (pressurePlate != null && !pressurePlate.isPressed)
        {
            pressurePlate.Press();
        }
    }

    void PickUp(Transform interactor)
    {
        if(pressurePlate!=null&&pressurePlate.isPressed)
        {
            pressurePlate.Release();
        }
        isHeld = true;
        transform.SetParent(interactor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    // ===== Place =====
    void Place()
    {
        isHeld = false;

        transform.SetParent(null);

        Transform cam = Camera.main.transform;

        Vector3 origin = cam.position;
        Vector3 direction = cam.forward;

        Vector3 placePosition;
        float safetyYOffset = 0.05f;
        // 1. Raycast từ tâm camera
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placeDistance))
        {
            // 2. Nếu trúng bề mặt → đặt tại điểm va chạm + offset
            Collider itemCol = GetComponent<Collider>();

            float offset = 0f;
            if (itemCol != null)
            {
                Vector3 extents = itemCol.bounds.extents;

                offset =
                    Mathf.Abs(hit.normal.x) * extents.x +
                    Mathf.Abs(hit.normal.y) * extents.y +
                    Mathf.Abs(hit.normal.z) * extents.z;
            }

            placePosition = hit.point + hit.normal * offset + Vector3.up * safetyYOffset;
        }
        else
        {
            // 3. Không trúng gì → đặt tại điểm xa nhất
            placePosition = origin + direction * placeDistance + Vector3.up * safetyYOffset;
        }

        transform.position = placePosition;

        // 4. Bật lại physics
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
}
