using UnityEngine;

public class PedestalEquip : MonoBehaviour,IInteractable
{
    [Header("References")]
    [SerializeField] private Transform player;          // Player transform
    [SerializeField] private GameObject pedestalStick;  // Stick trên bệ
    [SerializeField] private GameObject playerStick;    // Stick trên player
    [SerializeField] private Canvas promptCanvas;       // Canvas con của bệ
    [SerializeField] private string interactText;

    [Header("Settings")]
    [SerializeField] private float detectDistance = 3f; // khoảng cách để hiện prompt

    private bool playerInRange = false;

    void Start()
    {
        if (promptCanvas != null)
            promptCanvas.enabled = false;

        // giả sử ban đầu stick trên bệ hiện, stick trên player ẩn
        if (playerStick != null)
            playerStick.SetActive(false);
        if (pedestalStick != null)
            pedestalStick.SetActive(true);
    }

    void Update()
    {
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
    }

    public void ToggleStick()
    {
        bool playerHasStick = playerStick.activeSelf;

        if (playerHasStick)
        {
            // Drop stick về bệ
            playerStick.SetActive(false);
            pedestalStick.SetActive(true);
            Debug.Log("Stick dropped back to pedestal!");
        }
        else
        {
            // Equip stick từ bệ
            pedestalStick.SetActive(false);
            playerStick.SetActive(true);
            Debug.Log("Stick equipped!");
        }

        if (promptCanvas != null)
            promptCanvas.enabled = false;
    }

    public void Interact(Transform interactorTransform)
    {
        ToggleStick();
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
