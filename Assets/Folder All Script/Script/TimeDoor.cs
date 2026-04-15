using UnityEngine;
using TMPro;
using System.Collections;

public class TimeDoor : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private NormalDoor door;

    [Header("Timer")]
    public float openDuration = 5f;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    private bool isActive;

    void Start()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isActive) return;

        isActive = true;
        door.OpenDoor();

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        float timeLeft = openDuration;
        countdownText.gameObject.SetActive(true);

        while (timeLeft > 0)
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString();
            countdownText.color = Color.red;

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        countdownText.gameObject.SetActive(false);
        door.CloseDoor();
        isActive = false;
    }
}
