using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Animator animator;
    [Header("Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    [SerializeField] public bool isPressed = false;

    public void Press()
    {
        isPressed = true;

        if (animator != null)
        {
            animator.SetBool("IsDown", true);
        }

        onPressed?.Invoke();
    }

    public void Release()
    {
        isPressed = false;

        if (animator != null)
        {
            animator.SetBool("IsDown", false);
        }

        onReleased?.Invoke();
    }
    public void OnPress()
    {
        Debug.Log("da an");
    }
    public void OnRelease()
    {
        Debug.Log("da ve ban dau");
    }
}
