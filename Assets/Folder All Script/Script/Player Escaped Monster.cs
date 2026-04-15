using UnityEngine;

public class PlayerEscapedMonster : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;

    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (!other.CompareTag(playerTag)) return;


        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
        hasTriggered = true;
    }
}

