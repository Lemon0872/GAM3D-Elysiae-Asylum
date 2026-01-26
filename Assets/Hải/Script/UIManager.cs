using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject settingCanvas;

    public void OpenSetting()
    {
        mainCanvas.SetActive(false);
        settingCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        settingCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
}

