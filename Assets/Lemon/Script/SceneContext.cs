using UnityEngine;
using System.Collections.Generic;

public class SceneContext : MonoBehaviour
{
    [Header("Objects to disable during minigame")]
    public List<GameObject> disableOnMinigame;

    public void EnterMinigame()
    {
        foreach (var obj in disableOnMinigame)
            if (obj) obj.SetActive(false);
    }

    public void ExitMinigame()
    {
        foreach (var obj in disableOnMinigame)
            if (obj) obj.SetActive(true);
    }
}
