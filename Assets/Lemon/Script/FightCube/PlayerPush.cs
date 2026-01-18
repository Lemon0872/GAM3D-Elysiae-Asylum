using UnityEngine;

public class PlayerPush : MonoBehaviour,IInteractable
{
    public CubeController cube;
    public KeyCode interactKey = KeyCode.E;
    [SerializeField] private string interactText;
    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        cube.TryPush(transform);
    }

    void Update()
    {
        
    }
}
