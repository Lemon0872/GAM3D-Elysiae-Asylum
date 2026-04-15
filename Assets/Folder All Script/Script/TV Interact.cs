using UnityEngine;
using System.Collections;

public class TVInteract : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private string turnOnText = "E: Turn on";
    [SerializeField] private string turnOffText = "E: Turn off";

    private bool isOn;

    [Header("TV")]
    [SerializeField] private GameObject tvContent;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip staticClip;

    private Coroutine audioRoutine;

    void Start()
    {
        if (!audioSource)
            audioSource = GetComponentInChildren<AudioSource>();

        if (tvContent)
            isOn = tvContent.activeSelf;

        if (isOn)
            StartStatic();
    }

    public string GetInteractText()
    {
        return isOn ? turnOffText : turnOnText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        ToggleTV();
    }

    void ToggleTV()
    {
        isOn = !isOn;

        if (tvContent)
            tvContent.SetActive(isOn);

        if (audioRoutine != null)
            StopCoroutine(audioRoutine);

        audioRoutine = StartCoroutine(AudioSequence());
    }

    IEnumerator AudioSequence()
    {
        // Stop anything currently playing
        audioSource.Stop();
        audioSource.loop = false;

        // 🔘 Play click
        audioSource.clip = clickClip;
        audioSource.Play();

        yield return new WaitForSeconds(clickClip.length);

        if (isOn)
        {
            // 📺 Start static loop AFTER click
            StartStatic();
        }
    }

    void StartStatic()
    {
        audioSource.clip = staticClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
