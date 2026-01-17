using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public CubeController cube;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            cube.TryPush(transform);
        }
    }
}
