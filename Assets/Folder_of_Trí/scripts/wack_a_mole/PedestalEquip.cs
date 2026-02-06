using UnityEngine;
using TMPro;

public class PedestalEquip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;          // Player transform
    [SerializeField] private GameObject pedestalStick;  // Stick trên bệ (object con)
    [SerializeField] private GameObject playerStick;    // Stick trên player (ban đầu disable)
    [SerializeField] private Canvas promptCanvas;       // Canvas con của bệ chứa TMP

    [Header("Settings")]
    [SerializeField] private float detectDistance = 3f; // khoảng cách để hiện prompt

    private bool playerInRange = false;
    private bool equipped = false;

    void Start()
    {
        if (promptCanvas != null)
            promptCanvas.enabled = false;

        if (playerStick != null)
            playerStick.SetActive(false); // stick trên player ban đầu ẩn
    }

    void Update()
    {
        if (equipped) return;

        // Kiểm tra khoảng cách
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= detectDistance)
        {
            if (promptCanvas != null)
                promptCanvas.enabled = true;
            playerInRange = true;
        }
        else
        {
            if (promptCanvas != null)
                promptCanvas.enabled = false;
            playerInRange = false;
        }

        // Nếu player đang va chạm với bệ và nhấn F
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            EquipStick();
        }
    }

    private void EquipStick()
    {
        equipped = true;

        if (promptCanvas != null)
            promptCanvas.enabled = false;

        if (pedestalStick != null)
            pedestalStick.SetActive(false);

        if (playerStick != null)
            playerStick.SetActive(true);

        Debug.Log("Stick equipped!");
    }
}
