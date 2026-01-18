using UnityEngine;
using System.Collections;

public class HammerController : MonoBehaviour
{
    public float hitAngle = 30f;       // góc xoay khi đánh (trục X)
    public float hitDuration = 0.2f;   // thời gian giữ gậy ở góc xoay
    public float returnSpeed = 5f;     // tốc độ quay về vị trí ban đầu

    private Quaternion originalRotation; // rotation ban đầu
    private bool isHitting = false;

    void Start()
    {
        // Lưu rotation ban đầu của gậy
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Khi click chuột trái
        if (Input.GetMouseButtonDown(0) && !isHitting)
        {
            StartCoroutine(HitRoutine());
        }
    }

    IEnumerator HitRoutine()
    {
        isHitting = true;

        // Xoay gậy xuống trục X (màu đỏ)
        Quaternion hitRotation = originalRotation * Quaternion.Euler(-hitAngle, 0, 0);
        transform.localRotation = hitRotation;

        // Giữ gậy ở vị trí đánh trong một khoảng thời gian
        yield return new WaitForSeconds(hitDuration);

        // Quay gậy về vị trí ban đầu từ từ
        while (Quaternion.Angle(transform.localRotation, originalRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.localRotation = originalRotation;
        isHitting = false;
    }
}
