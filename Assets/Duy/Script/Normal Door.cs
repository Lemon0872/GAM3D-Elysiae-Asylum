using UnityEngine;

public class NormalDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string openText = "E to Open";
    [SerializeField] private string closeText = "E to Close";
    private Animator animator;
    private bool isOpen;
    [SerializeField] private AudioSource audioSource;

    [Header("Audio")]
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
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
        /*isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);*/
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);
        if (audioSource && openClip)
            audioSource.PlayOneShot(openClip);
    }

    public void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
        if (audioSource && closeClip)
            audioSource.PlayOneShot(closeClip);
    }

}
