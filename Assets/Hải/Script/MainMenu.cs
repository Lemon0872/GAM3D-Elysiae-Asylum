using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Chuyển sang scene chơi game
    public void PlayGame()
    {
        SceneManager.LoadScene("map_test"); 
        // Đổi "PlayScene" đúng tên scene của bạn
    }

    // Thoát game
    public void ExitGame()
    {
        Debug.Log("Game đã thoát!");

        Application.Quit();

        // Nếu đang chạy trong Unity Editor thì dừng Play Mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
