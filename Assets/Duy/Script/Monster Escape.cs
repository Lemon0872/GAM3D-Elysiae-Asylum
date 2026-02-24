using UnityEngine;

public class MonsterEscape : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private GameObject objectToEnable;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    [Header("Sound")]
    [SerializeField] private AudioClip glassBreakClip;
    [SerializeField] private float glassVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Chammmm");

        if (hasTriggered)
        {
            Debug.Log("[TriggerZone] Already triggered. Ignoring.");
            return;
        }

        if (!other.CompareTag(playerTag)) return;

        Debug.Log("[TriggerZone] Player stepped on trigger: " + gameObject.name);

        Vector3 soundPosition = Vector3.zero;

        if (objectToDisable != null)
        {
            soundPosition = objectToDisable.transform.position;

            objectToDisable.SetActive(false);
            Debug.Log("[TriggerZone] Disabled: " + objectToDisable.name);

            // 🔊 Play glass sound at object's position
            if (glassBreakClip != null)
            {
                AudioSource.PlayClipAtPoint(glassBreakClip, soundPosition, glassVolume);
                Debug.Log("[TriggerZone] Played glass sound.");
            }
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
