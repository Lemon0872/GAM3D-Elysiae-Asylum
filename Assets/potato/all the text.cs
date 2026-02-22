using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class allthetext : MonoBehaviour
{
    public TMP_Text UItext;
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    public triggerController controller;

    [Header("Trigger Settings")]
    public bool isRequired = true;

    private bool hasTriggered = false;

    void Start()
    {
        UItext.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(ShowDialogue());
        }
    }

    IEnumerator ShowDialogue()
    {
        UItext.gameObject.SetActive(true);

        foreach (DialogueLine line in dialogueLines)
        {
            UItext.text = line.text;
            yield return new WaitForSeconds(line.showTime);
        }

        UItext.gameObject.SetActive(false);
        gameObject.SetActive(false);

        controller.TriggerFinished(isRequired);
    }
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(2,5)]
    public string text;

    public float showTime = 3f;
}