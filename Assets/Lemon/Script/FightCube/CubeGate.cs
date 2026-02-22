using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeGate : MonoBehaviour,IInteractable
{
    public string CubemName;
    [SerializeField] private GameObject MainSceneObj;
    [SerializeField] private string interactText;
    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public void Interact(Transform interactorTransform)
    {
        SceneManager.LoadSceneAsync(CubemName,LoadSceneMode.Additive);
        MainSceneObj.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
