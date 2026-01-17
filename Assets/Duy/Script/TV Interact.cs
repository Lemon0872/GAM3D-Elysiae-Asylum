using UnityEngine;

public class TVInteract : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private string turnOnText = "E: Turn on";
    [SerializeField] private string turnOffText = "E: Turn off";
    private bool isOn;

    [Header("TV")]
    [SerializeField] private GameObject tvContent; 

    void Start()
    {
        if (tvContent != null)
            isOn = tvContent.activeSelf;
    }
    public string GetInteractText()
    {
        return isOn ? turnOffText : turnOnText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        ToggleTV();
    }

    void ToggleTV()
    {
        isOn = !isOn;

        if (tvContent != null)
            tvContent.SetActive(isOn);
    }
    
}
