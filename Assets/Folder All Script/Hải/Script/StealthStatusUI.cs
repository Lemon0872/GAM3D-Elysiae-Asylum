using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // QUAN TRỌNG

public class StealthStatusUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public bool isHidden = true;

    void Start()
    {
        UpdateStatus();
    }

    void Update()
    {
        // TEST bằng phím H (Input System mới)
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            isHidden = !isHidden;
            UpdateStatus();
        }
    }

    void UpdateStatus()
    {
        if (isHidden)
        {
            statusText.text = "HIDDEN";
            statusText.color = new Color(0.7f, 0.7f, 0.7f);
        }
        else
        {
            statusText.text = "DETECTED";
            statusText.color = Color.red;
        }
    }
}
