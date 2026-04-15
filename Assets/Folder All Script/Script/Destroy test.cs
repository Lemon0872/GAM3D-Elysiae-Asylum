using UnityEngine;

public class Destroytest : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public void Interact(Transform interactorTransform)
    {
        Test();
    }

    public void Test()
    {
        Destroy(gameObject);
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
