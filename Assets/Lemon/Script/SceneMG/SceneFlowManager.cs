using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneFlowManager : MonoBehaviour
{
    //LoadMinigame() LoadSceneAsync
    //LoadFullScene() Load bth
    //Return() gọi khi muốn về main scene từ LoadMinigame()
    public static SceneFlowManager Instance;

    [SerializeField] LoadingUI loadingUI;

    Stack<string> sceneStack = new();
    bool isTransitioning;
    private Scene previousScene;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region LOAD MINIGAME

    public void LoadMinigame(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(LoadRoutine(sceneName));
    }

    IEnumerator LoadRoutine(string sceneName)
    {
        isTransitioning = true;

        loadingUI.Show();

        AsyncOperation op =
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            loadingUI.SetProgress(op.progress);
            yield return null;
        }

        loadingUI.SetProgress(1f);

        yield return new WaitForSecondsRealtime(0.2f);

        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;
        previousScene = SceneManager.GetActiveScene();
        HideScene(previousScene);
        SceneManager.SetActiveScene(
            SceneManager.GetSceneByName(sceneName));

        sceneStack.Push(sceneName);

        //chuyển state
        GameStateManager.Instance.SetState(GameState.Minigame);

        loadingUI.Hide();
        isTransitioning = false;
    }
    public void LoadFullScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(LoadFullRoutine(sceneName));
    }

    IEnumerator LoadFullRoutine(string sceneName)
    {
        isTransitioning = true;

        loadingUI.Show();
        sceneStack.Clear();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            loadingUI.SetProgress(op.progress);
            yield return null;
        }

        loadingUI.SetProgress(1f);
        yield return new WaitForSecondsRealtime(0.2f);

        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        //quyết định state
        if (sceneName == "MainMenu")
            GameStateManager.Instance.SetState(GameState.Menu); 

        loadingUI.Hide();
        isTransitioning = false;
    }
    
    void HideScene(Scene scene)
    {
        var roots = scene.GetRootGameObjects();

        foreach (var go in roots)
            go.SetActive(false);
    }
    void ShowScene(Scene scene)
    {
        var roots = scene.GetRootGameObjects();

        foreach (var go in roots)
            go.SetActive(true);
    }

    #endregion

    #region RETURN

    public void Return()
    {
        if (isTransitioning) return;
        if (sceneStack.Count == 0) return;

        StartCoroutine(ReturnRoutine());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator ReturnRoutine()
    {
        isTransitioning = true;

        loadingUI.Show();

        string sceneName = sceneStack.Pop();
        ShowScene(previousScene);
        AsyncOperation op =
            SceneManager.UnloadSceneAsync(sceneName);

        while (!op.isDone)
            yield return null;

        if (sceneStack.Count == 0)
            GameStateManager.Instance.SetState(GameState.Gameplay);

        loadingUI.Hide();
        isTransitioning = false;
    }

    #endregion
}
