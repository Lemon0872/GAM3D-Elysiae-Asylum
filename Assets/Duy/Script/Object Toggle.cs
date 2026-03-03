using UnityEngine;
using UnityEngine.Events;

public class ObjectToggle : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    public void EnableObject()
    {
        if (targetObject == null) return;
        targetObject.SetActive(true);
    }

    public void DisableObject()
    {
        if (targetObject == null) return;
        targetObject.SetActive(false);
    }

    public void KillObject()
    {
        if (targetObject == null) return;
        StartCoroutine(DisableAfterDelay());
    }

    private System.Collections.IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        targetObject.SetActive(false);
    }

    public void ToggleObject()
    {
        if (targetObject == null) return;
        targetObject.SetActive(!targetObject.activeSelf);
    }

}