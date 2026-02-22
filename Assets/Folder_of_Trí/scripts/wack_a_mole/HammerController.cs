using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;   
using System.Collections;
using System.Collections.Generic;

public class HammerController : MonoBehaviour
{
    [Header("Hammer Settings")]
    public float hitAngle = 30f;
    public float hitDuration = 0.2f;
    public float returnSpeed = 5f;

    private Quaternion originalRotation;
    private bool isHitting = false;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset inputActions;      private InputAction wackAction;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI messageText;     

    void Awake()
    {
        var actionMap = inputActions.FindActionMap("HammerControl", throwIfNotFound: true);
        wackAction = actionMap.FindAction("Wack", throwIfNotFound: true);

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

    public void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
        }
        else
        {
            Debug.LogWarning("Chưa gán TextMeshProUGUI vào HammerController!");
        }
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
                    mole.OnHit();

                    if (mole.hasLetter)
                    {
                        char letter = mole.letter;
                        ShowMessage("Đã nhận ký tự: " + letter);

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
                        ShowMessage("Còn lại các ký tự: " + remainingStr);
                    }
                    else
                    {
                        ShowMessage("Đã đánh trúng mole không có chữ cái");
                    }
                }
                else
                {
                    ShowMessage("Đã đánh trúng vật thể không phải mole: " + hit.collider.name);
                }
            }
            else
            {
                ShowMessage("Không đánh trúng gì cả");
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
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                originalRotation,
                Time.deltaTime * returnSpeed
            );
            yield return null;
        }

        transform.localRotation = originalRotation;
        isHitting = false;
    }
}
