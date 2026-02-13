using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
    Gameplay,
    Minigame,
    Menu,
    Pause
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentState { get; private set; }

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
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Time.timeScale = pausedStates.Contains(newState) ? 0f : 1f;

        Debug.Log($"State → {newState}");
    }
}
