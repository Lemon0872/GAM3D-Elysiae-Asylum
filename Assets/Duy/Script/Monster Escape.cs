using UnityEngine;

public class MonsterEscape : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private GameObject objectToEnable;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            Debug.Log("[TriggerZone] Already triggered. Ignoring.");
            return;
        }

        if (!other.CompareTag(playerTag)) return;

        Debug.Log("[TriggerZone] Player stepped on trigger: " + gameObject.name);

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
            Debug.Log("[TriggerZone] Disabled: " + objectToDisable.name);
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
            Debug.Log("[TriggerZone] Enabled: " + objectToEnable.name);
        }

        hasTriggered = true;

        Debug.Log("[TriggerZone] Trigger completed.");
    }
}
