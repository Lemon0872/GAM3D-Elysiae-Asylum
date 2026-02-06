using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Chơi lại màn hiện tại
    public void Retry()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Quay về menu chính
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu"); 
        // đổi "MainMenu" đúng tên scene menu của bạn
    }
}
