using UnityEngine;

public class ScriptToggle : MonoBehaviour
{
    [SerializeField] private MonoBehaviour targetScript;

    public void EnableScript()
    {
        if (targetScript == null) return;
        targetScript.enabled = true;
    }

    public void DisableScript()
    {
        if (targetScript == null) return;
        targetScript.enabled = false;
    }

    public void ToggleScript()
    {
        if (targetScript == null) return;
        targetScript.enabled = !targetScript.enabled;
    }
}