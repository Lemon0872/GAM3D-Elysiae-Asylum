using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Chuyển sang scene chơi game
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene"); 
        // Đổi "PlayScene" đúng tên scene của bạn
    }

    // Thoát game
    public void ExitGame()
    {
        Application.Quit();

        // Dòng này chỉ để test trong Unity Editor
        Debug.Log("Game đã thoát!");
    }
}
