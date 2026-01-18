using UnityEngine;

public class NormalDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string openText = "E to Open";
    [SerializeField] private string closeText = "E to Close";
    private Animator animator;
    private bool isOpen;
    public void Interact(Transform interactorTransform)
    {
        ToggleDoor();
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public string GetInteractText()
    {
        return isOpen ? closeText : openText;
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
    }

}
