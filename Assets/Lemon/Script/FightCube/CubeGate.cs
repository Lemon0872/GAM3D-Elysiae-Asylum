using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeGate : MonoBehaviour,IInteractable
{
    public string cubeSceneName;
    [SerializeField] private string interactText;
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
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
        SceneFlowManager.Instance.LoadMinigame(cubeSceneName);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            anim.SetBool("IsGateOpen",true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            anim.SetBool("IsGateOpen",false);
        }
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
