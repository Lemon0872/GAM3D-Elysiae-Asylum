using UnityEngine;
using TMPro;

public class paper_appear : MonoBehaviour
{
    public GameObject paperUI;              // The Panel (UI)
    public TextMeshProUGUI interactPrompt;  // "Press E" text

    private bool playerInRange;
    private bool isOpen;

    [SerializeField] private string openText = "Press E to look at";
    [SerializeField] private string closeText = "Press E to stop looking";

    void Start()
    {
        paperUI.SetActive(false);
        interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TogglePaper();
        }
    }

    void TogglePaper()
    {
        isOpen = !isOpen;
        paperUI.SetActive(isOpen);
        interactPrompt.text = isOpen ? closeText : openText;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPrompt.gameObject.SetActive(true);
            interactPrompt.text = openText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPrompt.gameObject.SetActive(false);
            paperUI.SetActive(false);
            isOpen = false;
        }
    }
}