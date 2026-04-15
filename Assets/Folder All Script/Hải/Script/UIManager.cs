using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject settingCanvas;

    public TextMeshProUGUI title;



    public void OpenSetting()
    {
        mainCanvas.SetActive(false);
        settingCanvas.SetActive(true);
        title.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        settingCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        title.gameObject.SetActive(true);
    }

    public void Game()
    {
        SceneManager.LoadScene("map_test");
    }

        public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

