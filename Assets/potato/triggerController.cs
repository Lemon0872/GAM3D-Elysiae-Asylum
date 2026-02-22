using UnityEngine;

public class triggerController : MonoBehaviour
{
    public TriggerStage[] stages;

    private int currentStage = 0;

    void Start()
    {
        // Disable all triggers
        foreach (TriggerStage stage in stages)
        {
            stage.completedRequiredTriggers = 0;
            stage.requiredTriggersCount = stage.triggersInStage.Length;

            foreach (GameObject trigger in stage.triggersInStage)
            {
                trigger.SetActive(false);
            }
        }

        ActivateStage(0);
    }

    public void TriggerFinished(bool wasRequired)
    {
        TriggerStage stage = stages[currentStage];

        if (stage.requireAllTriggers)
        {
            if (wasRequired)
            {
                stage.completedRequiredTriggers++;

                if (stage.completedRequiredTriggers >= stage.requiredTriggersCount)
                {
                    MoveToNextStage();
                }
            }
        }
        else
        {
            // Stage progresses immediately when ANY required trigger finishes
            if (wasRequired)
            {
                MoveToNextStage();
            }
        }
    }

    void MoveToNextStage()
    {
        currentStage++;

        if (currentStage < stages.Length)
        {
            ActivateStage(currentStage);
        }
    }

    void ActivateStage(int index)
    {
        foreach (GameObject trigger in stages[index].triggersInStage)
        {
            trigger.SetActive(true);
        }
    }
}

[System.Serializable]
public class TriggerStage
{
    public GameObject[] triggersInStage;

    [Header("Stage Rules")]
    public bool requireAllTriggers = true;

    [HideInInspector] public int requiredTriggersCount;
    [HideInInspector] public int completedRequiredTriggers;
}