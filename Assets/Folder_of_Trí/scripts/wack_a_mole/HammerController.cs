using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class HammerController : MonoBehaviour
{
    public float hitAngle = 30f;
    public float hitDuration = 0.2f;
    public float returnSpeed = 5f;

    private Quaternion originalRotation;
    private bool isHitting = false;

    private InputAction wackAction;

    void Awake()
    {
        // Tạo action map và lấy action "Wack"
        var inputActionAsset = new Hammer();
        wackAction = inputActionAsset.HammerControl.Wack;
        wackAction.performed += ctx => OnWack();
    }

    void OnEnable()
    {
        wackAction.Enable();
    }

    void OnDisable()
    {
        wackAction.Disable();
    }

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    private void OnWack()
    {
        if (!isHitting)
        {
            StartCoroutine(HitRoutine());

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Mole mole = hit.collider.GetComponent<Mole>();
                if (mole != null)
                {
                    mole.OnHit(); // xử lý chính

                    if (mole.hasLetter)
                    {
                        char letter = mole.letter;
                        Debug.Log("✅ Đã nhận ký tự: " + letter);

                        // Tính toán các ký tự còn lại
                        string target = GameManager.Instance.targetWord;
                        List<char> collected = GameManager.Instance.collectedLetters;
                        List<char> remaining = new List<char>();

                        foreach (char c in target)
                        {
                            if (!collected.Contains(c))
                            {
                                remaining.Add(c);
                            }
                        }

                        string remainingStr = string.Join(", ", remaining);
                        Debug.Log("🔤 Còn lại các ký tự: " + remainingStr);
                    }
                    else
                    {
                        Debug.Log("💥 Đã đập mole không có chữ cái");
                    }
                }
                else
                {
                    Debug.Log("❌ Raycast trúng vật thể không phải mole: " + hit.collider.name);
                }
            }
            else
            {
                Debug.Log("⚠️ Không trúng gì cả khi raycast");
            }
        }
    }


    IEnumerator HitRoutine()
    {
        isHitting = true;

        Quaternion hitRotation = originalRotation * Quaternion.Euler(-hitAngle, 0, 0);
        transform.localRotation = hitRotation;

        yield return new WaitForSeconds(hitDuration);

        while (Quaternion.Angle(transform.localRotation, originalRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * returnSpeed);
            yield return null;
        }
        
        transform.localRotation = originalRotation;
        Debug.Log("đã trở về");
        isHitting = false;
    }


}
