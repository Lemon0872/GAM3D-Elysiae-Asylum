using UnityEngine;
using System.Collections.Generic;

public class CameraCutsceneCaster : MonoBehaviour
{
    public CameraCutsceneController controller;
    public List<CameraCutsceneBinding> cutscenes;

    public void Cast(int index)
    {
        Debug.Log($"da cast {index}");
        if (index < 0 || index >= cutscenes.Count)
            return;

        var binding = cutscenes[index];
        if(binding.hasCast&&binding.OnDisable) return;
        binding.hasCast=true;
        controller.Cast(binding);
    }
}
