using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Slider progressBar;

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void SetProgress(float value)
    {
        progressBar.value = value;
    }
}
