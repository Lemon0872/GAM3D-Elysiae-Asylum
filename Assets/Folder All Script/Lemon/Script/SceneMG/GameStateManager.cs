using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    Gameplay,
    Minigame,
    Menu,
    Pause
}

[System.Serializable]
public class MiniGameEntry
{
    //GameStateManager.Instance.MarkMiniGameCompleted(id"id là biến string trong scene minigame muốn mark, id phải khớp với ít nhất 1 ptử trong miniGames");
    public string ID;
    public bool isCompleted;
    public UnityEvent OnCompleted;
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public GameState CurrentState { get; private set; }
    [SerializeField] 
    List<MiniGameEntry> miniGames = new();
    [Tooltip("tên scene sẽ trở về khi complete minigame")]
    [SerializeField] string mainSceneName;
    HashSet<GameState> pausedStates = new()
    {
        GameState.Pause
    };

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetState(GameState.Gameplay);
        SceneManager.sceneUnloaded += HandleSceneUnloaded;
    }
    
    void HandleSceneUnloaded(Scene scene)
    {
            CheckCompletedMiniGames();
            Debug.Log("thuc hien check");
    }

    void CheckCompletedMiniGames()
    {
        foreach (var entry in miniGames)
        {
            if (entry.isCompleted)
            {
                entry.OnCompleted?.Invoke();
                entry.isCompleted = false;
            }
        }
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Time.timeScale = pausedStates.Contains(newState) ? 0f : 1f;

        Debug.Log($"State → {newState}");
    }
    public void MarkMiniGameCompleted(string id)
    {
        var entry = miniGames.Find(m => m.ID == id);

        if (entry != null)
        {
            entry.isCompleted = true;
        }
    }
    public void gamestatetest()
    {
        Debug.Log("da win roi");
    }
}
