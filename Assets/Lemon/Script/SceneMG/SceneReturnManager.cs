using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneReturnManager : MonoBehaviour
{
    public static SceneReturnManager Instance;
    public SceneContext mainContext;
    Stack<Scene> sceneStack = new();

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (mode == LoadSceneMode.Additive)
    {
        sceneStack.Push(scene);
        SceneManager.SetActiveScene(scene);

        mainContext?.EnterMinigame();
    }
}

public void Return()
{
    if (sceneStack.Count == 0)
        return;

    Scene current = sceneStack.Pop();
    SceneManager.UnloadSceneAsync(current);

    if (sceneStack.Count == 0)
        mainContext?.ExitMinigame();
}
}
